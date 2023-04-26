using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public String Date { get; set; }

        private object? _selectedItem;
        public object? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private int _selectedItemIndex;

        private MaynotTile _selectedTile;

        public MaynotTile SelectedTile
        {
            get { return _selectedTile; }
            set
            {
                _selectedTile = value;
                OnPropertyChanged(nameof(SelectedTile));
            }
        }

        public ICommand RadioButtonCheckedCommand { get; set; }
        public ICommand PlaceItemCommand { get; set; }

        private ObservableCollection<MaynotTile> _letehetoElemek;

        public ObservableCollection<MaynotTile> LetehetoElemek
        {
            get { return _letehetoElemek; }
            set
            {
                _letehetoElemek = value;
                OnPropertyChanged(nameof(LetehetoElemek));
            }
        }



        public MaynotViewModel(MaynotGameModel model)
        {
            _model = model;
            _model.DayElapsed += new EventHandler<TimeElapsedEventArgs>(Day_Elapsed); 
            

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

            LetehetoElemek = new ObservableCollection<MaynotTile>
            {
                new Road(),
                new ResidentialZone(),
                new IndustrialZone(),
                new ServiceZone(),
                new PoliceStation(),
                new Stadium(),
                new School(),
                new University(),
                new Forest()
            };

        }

        private void Day_Elapsed(object sender, TimeElapsedEventArgs e)
        {
            Date = e.newDate.ToString("yyyy MMMM dd");
            OnPropertyChanged(nameof(Date));
            Debug.WriteLine($"new Day: {Date}");
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

        private void OnRadioButtonChecked(object? selectedItem)
        {
            if (selectedItem is MaynotTile maynotTile)
            {
                // Your logic to handle the click event
                Debug.WriteLine($"Klikkelve: {maynotTile.Name}");
                SelectedTile = maynotTile;
            }
            else
            {
                throw new ArgumentException("Olyan elemen jött be 'OnRadioButtonChecked' esemény amely nem MaynotTile típusú!");
            }
        }


        private void OnPlaceItem(object? selectedItem)
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
                    Fields.Add(new Empty
                    {
                        X = i,
                        Y = j,
                        ClickCommand = new DelegateCommand((param)=> PlaceTile(param as MaynotTile))
                    });
                }
            }
            OnPropertyChanged(nameof(Money));
            UpdateTable();
        }

        private void UpdateTable()
        {
            int numOfRoads = 0;
            for (int i = 0; i < Fields.Count; i++) // Előszőr friss állapotra hozzuk a mi Fields-ünket a Model tábla alapján
            {
                MaynotTile tile = Fields[i];
                MaynotTile modelbolTile = GetTileAtCoordinatesFromModel(tile.X, tile.Y);
                Fields[i] = modelbolTile;
                
            }
            foreach (MaynotTile tile in Fields) // Aztán frissítjük a Road sprite-okat
            {
                if (tile is Road road)
                {
                    //Debug.WriteLine("A Fieldsben talalt ut koodinatai: " + road.X + " " + road.Y);
                    bool north = false, east = false, south = false, west = false;
                    int fieldSize = (int)Math.Sqrt(Fields.Count);

                    if (road.Y > 0 && GetFieldAtCoordinates(road.X, road.Y - 1) is not Empty)
                    {
                        west = true;
                    }
                    if (road.Y < (fieldSize - 1) && GetFieldAtCoordinates(road.X, road.Y + 1) is not Empty)
                    {
                        east = true;
                    }
                    if (0 < road.X && GetFieldAtCoordinates(road.X - 1, road.Y) is not Empty)
                    {
                        north = true;
                    }
                    if (road.X < (fieldSize - 1) && GetFieldAtCoordinates(road.X + 1, road.Y) is not Empty)
                    {
                        south = true;
                    }
                    road.SetRoadSprite(north, east, south, west);
                    numOfRoads++;
                }
            }
            //Debug.WriteLine("Number of Roads: " + numOfRoads);
            OnPropertyChanged(nameof(Money));
            OnPropertyChanged(nameof(Fields));
        }

        private void PlaceTile(MaynotTile tile)
        {
            Debug.WriteLine("Clicked on: " + tile.X + " " + tile.Y);
            //Debug.WriteLine("Selected Tile is: " + SelectedTile.GetType());
            if (SelectedTile is Road)
            {
                _model.placeRoad(tile.X, tile.Y);
            }
            else if (SelectedTile is Zone zone)
            {
                if (zone is ResidentialZone)
                {
                    _model.placeResidentialZone(tile.X, tile.Y);
                }
                else if (zone is IndustrialZone)
                {
                    _model.placeIndustrialZone(tile.X, tile.Y);
                }
                else if (zone is ServiceZone)
                {
                    _model.placeServiceZone(tile.X, tile.Y);
                }
            }
            else if (SelectedTile is Facility facility)
            {
                if (facility is PoliceStation)
                {
                    _model.placePoliceStation(tile.X, tile.Y);
                }
                else if (facility is Stadium)
                {
                    _model.placeStadium(tile.X, tile.Y);
                }
                else if (facility is School)
                {
                    _model.placeSchool(tile.X, tile.Y);
                }
                else if (facility is University)
                {
                    _model.placeUni(tile.X, tile.Y);
                }
            }
            else if (SelectedTile is Forest)
            {
                _model.placeForest(tile.X, tile.Y);
            }

            MaynotTile modelbolTile = GetTileAtCoordinatesFromModel(tile.X, tile.Y);

            int fieldMeret = (int) (Math.Sqrt(Fields.Count));
            //Fields[tile.X * fieldMeret + tile.Y] = modelbolTile; // Itt nagyon gagyin felülírjuk a Fields listában lévő MaynotTile-t a mi tile-unkra
            //OnPropertyChanged(nameof(Fields));
            UpdateTable();

        }

        private MaynotTile ModelTileToMaynotTile(Tile tile) //Itt nem fogjuk az újonnan létrehozott instance-nek átadni a pozíciót a táblán
        {
            if (tile is MaynotPersistence.Empty) return new Empty();
            if (tile is MaynotPersistence.Road) return new Road(30);
            if (tile is MaynotPersistence.ResidentialZone) return new ResidentialZone();
            if (tile is MaynotPersistence.IndustrialZone) return new IndustrialZone();
            if (tile is MaynotPersistence.ServiceZone) return new ServiceZone();
            if (tile is MaynotPersistence.Forest) return new Forest(10, 10);
            if (tile is MaynotPersistence.PoliceStation) return new PoliceStation();
            if (tile is MaynotPersistence.Stadium) return new Stadium();
            if (tile is MaynotPersistence.School) return new School();
            if (tile is MaynotPersistence.University) return new University();
            
            return new Road(30);
        }

        private MaynotTile GetTileAtCoordinatesFromModel(int x, int y)
        {
            MaynotTile tile = ModelTileToMaynotTile(_model.GameBoard[x, y]);
            tile.X = x;
            tile.Y = y;
            tile.ClickCommand = new DelegateCommand((param) => PlaceTile(param as MaynotTile));
            return tile;
        }

        public MaynotTile GetFieldAtCoordinates(int x, int y)
        {
            int sorHossz = (int) Math.Sqrt(Fields.Count);
            return Fields[x * sorHossz + y];
        }
        public void SetFieldAtCoordinates(int x, int y, MaynotTile tile)
        {
            int sorHossz = (int)Math.Sqrt(Fields.Count);
            Fields[x * sorHossz + y] = tile;
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
