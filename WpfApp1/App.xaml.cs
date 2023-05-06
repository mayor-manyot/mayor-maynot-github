using Maynot.WPF.View;
using Maynot.WPF.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MaynotModel;
using MaynotPersistence;

namespace Maynot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private MaynotModel.MaynotGameModel _model = null!;
        private MaynotViewModel _viewModel = null!;
        private MainWindow _view = null!;

        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new MaynotGameModel(new FilePersistence());
            _model.catastropheHappened += new EventHandler<MaynotEventArg>(Model_CatastropheHappened);
            _model.GameLoaded += new EventHandler<MaynotEventArg>(Model_GameLoaded);
            _model.newGame();

            //ViewModel példányosítás előszőr
            _viewModel = new MaynotViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            //View példányosítás
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz

            this.MainWindow = _view;

            // Show the MainWindow
            _view.Show();
        }

        #region Model event handlers
        private void Model_CatastropheHappened(object? sender, MaynotEventArg e)
        {
            _viewModel.CatastropheHappened(e.X, e.Y);
        }
        private void Model_GameLoaded(object? sender, MaynotEventArg e)
        {
            _viewModel?.GameWasLoaded();
        }
        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.newGame();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Maynot tábla betöltése";
                openFileDialog.Filter = "Maynot tábla|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await _model.LoadGameAsync(openFileDialog.FileName);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Maynot", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Maynot tábla betöltése";
                saveFileDialog.Filter = "Maynot tábla|*.stl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Maynot", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object? sender, CancelEventArgs e)
        {

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Maynot", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

            }
        }

        #endregion

    }
}
