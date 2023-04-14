using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MaynotModel;
using MaynotPersistence;

namespace Maynot.WPF.ViewModel
{
    class MaynotViewModel : ViewModelBase
    {
        private MaynotGameModel _model;
        public ObservableCollection<MaynotTile> Fields { get; set; }
        public float Money { get { return _model.Money; } }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private int _selectedItemIndex;

        public ICommand RadioButtonCheckedCommand { get; set; }
        public ICommand PlaceItemCommand { get; set; }



        public MaynotViewModel(MaynotGameModel model)
        {
            _model = model;

            

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());
            SlowerGameCommand = new DelegateCommand(param => OnSlowerGame());
            SpeedUpGameCommand = new DelegateCommand(param => OnSpeedUpGameCommand());
            PauseGameCommand = new DelegateCommand(param => OnPauseGameCommand());
            ResumeGameCommand = new DelegateCommand(param => OnResumeGame());

            Fields = new ObservableCollection<MaynotTile>();
            FullyRefreshTable();

            RadioButtonCheckedCommand = new DelegateCommand(OnRadioButtonChecked);
            PlaceItemCommand = new DelegateCommand(OnPlaceItem);

        }

        private void OnResumeGame()
        {
            _model.resumeTime();
        }

        private void OnPauseGameCommand()
        {
            _model.stopTime();
        }

        private void OnSpeedUpGameCommand()
        {
            _model.speedUpTime();
        }

        private void OnSlowerGame()
        {
            _model.slowTime();
        }

        private void OnRadioButtonChecked(object selectedItem)
        {
            
            SelectedItem = selectedItem;
            if (selectedItem is RadioButton radioButton)
            {
                _selectedItemIndex = Int32.Parse((String)radioButton.Tag);
            }
        }

        private void OnPlaceItem(object selectedItem)
        {
            // Handle placing the selected item on the grid
        }

        public void FullyRefreshTable()
        {
            Fields.Clear();
            for (Int32 i = 0; i < _model.GameBoard.GetLength(0); i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.GameBoard.GetLength(0); j++)
                {
                    Fields.Add(new MaynotTile
                    {
                        X = i,
                        Y = j,
                        ClickCommand = new DelegateCommand((param)=> {
                            
                            Debug.WriteLine("about to place");
                            MaynotTile tile = param as MaynotTile;
                            Debug.WriteLine("Clicked on: " + tile.X + " " + tile.Y);
                            if (_selectedItemIndex == 0)
                            {
                                _model.placeRoad(tile.X, tile.Y);
                            }
                            else if (_selectedItemIndex == 1)
                            {
                                _model.placeResidentialZone(tile.X, tile.Y);
                                Debug.WriteLine("Place residental!!");
                            }
                            else if (_selectedItemIndex == 2)
                            {
                                _model.placeServiceZone(tile.X, tile.Y);
                            }
                            else if (_selectedItemIndex == 3)
                            {
                                _model.placeIndustrialZone(tile.X, tile.Y);
                            }
                            
                            tile.Name = ModelTileToMaynotTile(_model.GameBoard[tile.X, tile.Y]).Name;
                            //UpdateTable();
                        })
                    });
                }
            }
            OnPropertyChanged(nameof(Money));
            UpdateTable();
        }

        private void UpdateTable()
        {
            Int32[] voltak;
            foreach (MaynotTile tile in Fields) 
            {
                if (_model.GameBoard[tile.X, tile.Y] != null)
                {
                    Type obj = _model.GameBoard[tile.X, tile.Y].GetType();
                    Debug.WriteLine("Típusa: " + obj.ToString());
                    if (obj == typeof(ResidentialZone))
                    {
                        Debug.WriteLine("Residental in ViewModel!: " + ModelTileToMaynotTile(_model.GameBoard[tile.X, tile.Y]).Name);
                    }
                    
                }
                string nam = ModelTileToMaynotTile(_model.GameBoard[tile.X, tile.Y]).Name;
                tile.Name = nam;
                Debug.WriteLine("Amit berak: " + nam);
                Debug.WriteLine("Field mérete: " + Fields.Count());
                int sorfolytonos = tile.X * 30 + tile.Y;
            }

            OnPropertyChanged(nameof(Money));
            OnPropertyChanged(nameof(Fields));
        }

        private MaynotTile ModelTileToMaynotTile(Tile tile)
        {
            if (tile is MaynotPersistence.Empty) return new Empty();
            if (tile is MaynotPersistence.Road) return new Road(30);
            if (tile is MaynotPersistence.ResidentialZone)
            {
                Debug.WriteLine("Residental zóna felismerve a konverterben!");
                return new Zone(30, 30, ZoneType.RESIDENTIAL);
            };
            if (tile is MaynotPersistence.IndustrialZone) return new Zone(30, 30, ZoneType.INDUSTRIAL);
            if (tile is MaynotPersistence.ServiceZone) return new Zone(30, 30, ZoneType.SERVICE);
            return new Road(30);
        }

        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitGameCommand { get; private set; }

        public DelegateCommand SlowerGameCommand { get; private set; }

        public DelegateCommand SpeedUpGameCommand { get; private set; }

        public DelegateCommand PauseGameCommand { get; private set; }

        public DelegateCommand ResumeGameCommand { get; private set; }


        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler? LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler? SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler? ExitGame;

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }



        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
