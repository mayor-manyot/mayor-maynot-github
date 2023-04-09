using Maynot.WPF.View;
using Maynot.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Maynot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private MaynotViewModel _viewModel = null!;
        private MainWindow _view = null!;

        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //ViewModel példányosítás előszőr
            _viewModel = new MaynotViewModel();

            //View példányosítás
            _view = new MainWindow();
            _view.DataContext = _viewModel;

            this.MainWindow = _view;

            // Show the MainWindow
            _view.Show();
        }

    }
}
