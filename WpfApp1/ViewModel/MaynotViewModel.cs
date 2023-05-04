using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
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
        public ObservableCollection<Person> SatisfactionsInInspectedZone { get; set; }
        public String InspectedZoneCapacity { get; set; }
        public String InspectedZoneFullness { get; set; }
        public String InspectedZoneLevel { get; set; }
        public float Money { get { return _model.Money; } }
        public float ResidentalTax { get { return _model.ResidentalTax; } }
        public float ServiceTax { get { return _model.ServiceTax; } }
        public float IndustrialTax { get { return _model.IndustrialTax; } }
        public String Date { get; set; }
        public float Speed { get { return _model.Speed; } }
        public int Population { get { return _model.Population; } }
        public float Satisfaction { get { return _model.Population; } }

        private MaynotTile? _selectedTile;

        //Null hogyha éppen nincs semmi kiválasztva
        public MaynotTile? SelectedTile
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
            _model.GameSpeedChanged += new EventHandler<EventArgs>(Speed_Changed);
            _model.UpdatePopulation += new EventHandler<EventArgs>(Population_Changed);
            _model.InspectedTileChanged += new EventHandler<InspectedTileChangedEventArgs>(InspectedTile_Changed);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());
            SlowerGameCommand = new DelegateCommand(param => OnSlowerGame());
            SpeedUpGameCommand = new DelegateCommand(param => OnSpeedUpGameCommand());
            PauseGameCommand = new DelegateCommand(param => OnPauseGameCommand());
            ResumeGameCommand = new DelegateCommand(param => OnResumeGame());
            ResidentalTaxNumericUpCommand = new DelegateCommand(param => OnResidentalTaxNumericUp());
            ResidentalTaxNumericDownCommand = new DelegateCommand(param => OnResidentalTaxNumericDown());
            ServiceTaxNumericUpCommand = new DelegateCommand(param => OnServiceTaxNumericUp());
            ServiceTaxNumericDownCommand = new DelegateCommand(param => OnServiceTaxNumericDown());
            IndustrialTaxNumericUpCommand = new DelegateCommand(param => OnIndustrialTaxNumericUp());
            IndustrialTaxNumericDownCommand = new DelegateCommand(param => OnIndustrialTaxNumericDown());
            ClearCurrentlySelectedTileCommand = new DelegateCommand(param => OnClearCurrentlySelectedTile());
            OpenHelpPopupCommand = new DelegateCommand(param => OnOpenHelpPopupCommand());
            SelectBulldozerCommand = new DelegateCommand(param => OnSelectBulldozerCommand());
            UpgradeInspectedZoneCommand = new DelegateCommand(param => OnUpgradeInspectedZone());

            Fields = new ObservableCollection<MaynotTile>();
            SatisfactionsInInspectedZone = new ObservableCollection<Person>();
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
            //DebugTriggerCatastropheHappened(6,6);
        }

        private void Speed_Changed(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Speed));
        }
        private void InspectedTile_Changed(object sender, InspectedTileChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                SatisfactionsInInspectedZone.Clear();
            });
            if (e._tile is MaynotPersistence.Zone zone)
            {
                InspectedZoneCapacity = zone.Capacity.ToString();
                InspectedZoneFullness = zone.PeopleIndexes.Count().ToString();
                InspectedZoneLevel = ((int)zone.Level).ToString();
                foreach (var person in zone.GetPeoples(_model.Citizens))
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SatisfactionsInInspectedZone.Add(new Person
                        {
                            Satisfaction = person.Satisfaction
                        });
                    });
                    
                }
            }
            else
            {
                InspectedZoneCapacity = InspectedZoneFullness = InspectedZoneLevel = String.Empty;
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    SatisfactionsInInspectedZone.Clear();
                });
            }
            OnPropertyChanged(nameof(InspectedZoneCapacity));
            OnPropertyChanged(nameof(InspectedZoneFullness));
            OnPropertyChanged(nameof(InspectedZoneLevel));
        }
        private void Population_Changed(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Population));
            Debug.WriteLine($"Population Changed: {Population}");
        }
        private void Day_Elapsed(object sender, TimeElapsedEventArgs e)
        {
            Date = e.newDate.ToString("yyyy MMMM dd");
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Money));
            OnPropertyChanged(nameof(Satisfaction));
            Debug.WriteLine($"new Day: {Date}, New Budget: {Money}");
        }
        private void OnResidentalTaxNumericUp()
        {
            _model.setResidentalTax(_model.ResidentalTax + 1);
            OnPropertyChanged(nameof(ResidentalTax));
        }
        private void OnUpgradeInspectedZone()
        {
            _model.UpgradeInspectedZone();
        }
        private void OnResidentalTaxNumericDown()
        {
            _model.setResidentalTax(_model.ResidentalTax - 1);
            OnPropertyChanged(nameof(ResidentalTax));
        }
        private void OnIndustrialTaxNumericUp()
        {
            _model.setIndustrialTax(_model.IndustrialTax + 1);
            OnPropertyChanged(nameof(IndustrialTax));
        }
        private void OnIndustrialTaxNumericDown()
        {
            _model.setIndustrialTax(_model.IndustrialTax - 1);
            OnPropertyChanged(nameof(IndustrialTax));
        }
        private void OnServiceTaxNumericUp()
        {
            _model.setServiceTax(_model.ServiceTax + 1);
            OnPropertyChanged(nameof(ServiceTax));
        }
        private void OnServiceTaxNumericDown()
        {
            _model.setServiceTax(_model.ServiceTax - 1);
            OnPropertyChanged(nameof(ServiceTax));
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

        private void OnClearCurrentlySelectedTile()
        {
            SelectedTile = null;
        }

        //TODO
        private void OnOpenHelpPopupCommand()
        {
            throw new NotImplementedException("Nincs még Help Popup!");
        }

        private void OnSelectBulldozerCommand()
        {
            SelectedTile = new Bulldozer(); 
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
            else if (SelectedTile is Bulldozer)
            {
                _model.destroyRoad(tile.X, tile.Y);
                //_model.DestroyTile(tile.x, tile.Y);
            }
            else if (SelectedTile is null)
            {
                _model.InspectZone(tile.X, tile.Y);
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

        private async void DebugTriggerCatastropheHappened(int x, int y)
        {
            await Task.Delay(12000); // 12 seconds delay
            //_model.catastrophe();
            CatastropheHappened(x, y); //Direktbe Viewmodel fgv.-t is hívhatunk a Debughoz
        }

        public async void CatastropheHappened(int x, int y)
        {
            int radius = 5;
            List<MaynotTile> affectedTiles = new List<MaynotTile>();

            for (int i = x - radius; i <= x + radius; i++)
            {
                for (int j = y - radius; j <= y + radius; j++)
                {
                    if (i >= 0 && j >= 0 && i < _model.GameBoardSize && j < _model.GameBoardSize)
                    {
                        MaynotTile tile = GetFieldAtCoordinates(i, j);
                        if (tile != null)
                        {
                            affectedTiles.Add(tile);
                            tile.IsFlameVisible = Visibility.Visible;
                        }
                    }
                }
            }

            OnPropertyChanged(nameof(Fields));

            await Task.Delay(3300); // Várunk aztán eltüntetjü a Flame overlayt

            foreach (MaynotTile tile in affectedTiles)
            {
                tile.IsFlameVisible = Visibility.Hidden;
            }

            OnPropertyChanged(nameof(Fields));
            Application.Current.Dispatcher.Invoke(() => UpdateTable());
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
        public DelegateCommand ResidentalTaxNumericUpCommand { get; private set; }
        public DelegateCommand ResidentalTaxNumericDownCommand { get; private set; }
        public DelegateCommand ServiceTaxNumericUpCommand { get; private set; }
        public DelegateCommand ServiceTaxNumericDownCommand { get; private set; }
        public DelegateCommand IndustrialTaxNumericUpCommand { get; private set; }
        public DelegateCommand IndustrialTaxNumericDownCommand { get; private set; }
        public DelegateCommand ClearCurrentlySelectedTileCommand { get; private set; }
        public DelegateCommand OpenHelpPopupCommand { get; private set; }
        public DelegateCommand SelectBulldozerCommand { get; private set; }
        public DelegateCommand UpgradeInspectedZoneCommand { get; private set; }




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
