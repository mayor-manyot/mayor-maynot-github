using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MaynotPersistence;

namespace MaynotModel
{
    public class MaynotGameModel
    {
        private IPersistence _dataAccess;
        private MaynotGameState _state;
        
        public event EventHandler<MaynotEventArg>? SaveGame;
        public event EventHandler<MaynotEventArg>? LoadGame;
        public event EventHandler<MaynotEventArg>? ExitGame;
        public event EventHandler<MaynotEventArg>? TilePlaced;
        public event EventHandler<MaynotEventArg>? GeneralInfoAsked;
        public event EventHandler<TimeElapsedEventArgs>? DayElapsed;
        public event EventHandler<MaynotEventArg>? catastropheHappened;
        public event EventHandler<EventArgs>? GameSpeedChanged;
        public event EventHandler<EventArgs>? UpdatePopulation;

        public float Money { get => _state.money; set => _state.money = value; }
        public float Speed { get { return _state.gameSpeed; }  }
        public int Population { get { return _state.citizens.Count; } }
        public int ResidentalTax { get { return _state._residentalTax; } }
        public int ServiceTax { get { return _state._serviceTax; } }
        public int IndustrialTax { get { return _state._industrialTax; } }
        public double AverageSatisfaction { get { return _state._averageSatisfaction; } }
        public DateTime Time { get => _state.time; set => _state.time = value; }
        public int GameSpeed { get => _state.gameSpeed; set => _state.gameSpeed = value; }
        public Tile[,] GameBoard { get => _state.gameBoard; set => _state.gameBoard = value; }
        public int GameBoardSize { get => _state.size; }
        public List<Person> Citizens { get => _state.citizens; set => _state.citizens = value; }

        public MaynotGameModel(IPersistence dataAccess)
        {
            _dataAccess = dataAccess;
            _state = new MaynotGameState(0);
        }

        public void newGame()
        {
            _state = new MaynotGameState(30);
            _state.money = 100000;
            _state.gameSpeed = 1;
            _state.yearTracker = 1;
            _state.weakTracker = 0;
            _state._residentalTax = 69;
            _state._serviceTax = 4;
            _state._industrialTax = 95;
            _state.startX = GameBoardSize / 2 - 1;
            _state.startY = 0;
            //azt van használva a lehelyezésnél
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    _state.gameBoard[i, j] = new Empty();
                }
            }
            _state.gameBoard[_state.startX, _state.startY] = new Road();
            forestGenerator();

            _state.timer.Elapsed += Timer_Elapsed;
            _state.timer.Start();
            Debug.WriteLine("New game");
        }

        private void OnTimeElapsed()
        {
            DayElapsed?.Invoke(this, new TimeElapsedEventArgs(_state.time));
            UpdatePopulation?.Invoke(this, new EventArgs());
        }

        private async void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _state.time = _state.time.AddDays(1); // eltelik egy nap
            // naponta történõ eseményeknek nem kell ellenõrzés, a tick naponta van
            buildingEffects();
            growForests();
                

            if ((int)_state.time.DayOfWeek == 1) // Hétfõ lett, eltelt egy új hét
            {
                await Task.Run(() => movingIn());
                // minden hetente elõforduló esemény meghívása itt
                
            }

            if (_state.time.Day == 1) // minden hó elsején
            {
                // havonta meghívandó események metódusai
                catastrophe();
            }

            if (_state.time.Month == 1 && _state.time.Day == 1) // minden év eslõ napja
            {
                _state.money = _state.money - _state.expense;
                taxCollection();
            }

            OnTimeElapsed(); // új nap dátuma elküldése ViewModelnek a kinézetre
        }

        private async Task movingIn()
        {
            Random r = new Random();
            int newCitizens = r.Next(0, 25);
            for(int i=0; i<newCitizens; i++)
            {
                Person? p = null;
                int age = r.Next(18, 65);
                int satisfaction = r.Next(30, 100);
                int entryValue = r.Next(satisfaction, 100);
                (int, int) t = _state.getFreeResidentalZone();
                (int, int) w = _state.getFreeWorkZone(t.Item1, t.Item2);

                if (t.Item1 != -1 && t.Item2 != -1 && w.Item1 != -1 && w.Item2 != -1)
                {
                    p = new Person(satisfaction, age, t, w, Level.ELEMENTARY);
                }

                if(p != null && _state.guaranteedPopulation)
                {
                    _state.citizens.Add(p);
                    Zone? residentalZone = GameBoard[t.Item1, t.Item2] as Zone;
                    Zone? workZone = GameBoard[w.Item1, w.Item2] as Zone;
                    if (residentalZone != null && workZone != null)
                    {
                        residentalZone.PeopleIndexes.Add(_state.citizens.Count);
                        workZone.PeopleIndexes.Add(_state.citizens.Count);
                    }
                }
                else if(p != null && _state.calculateZoneAttractiveness(p) < entryValue)
                {
                    _state.citizens.Add(p);
                    Zone? residentalZone = GameBoard[t.Item1, t.Item2] as Zone;
                    Zone? workZone = GameBoard[w.Item1, w.Item2] as Zone;
                    if (residentalZone != null && workZone != null)
                    {
                        residentalZone.PeopleIndexes.Add(_state.citizens.Count);
                        workZone.PeopleIndexes.Add(_state.citizens.Count);
                    }
                }
                
                if(_state.citizens.Count < 500)
                {
                    _state.guaranteedPopulation = false;
                }
            }
        }

        public void stopTime()
        {
            if (_state.gameSpeed == 0) return;
            _state.timer.Stop();
            _state.prevGameSpeed = _state.gameSpeed;
            _state.gameSpeed = 0;
            GameSpeedChanged?.Invoke(this, new EventArgs());
        }

        public void resumeTime()
        {
            if (_state.gameSpeed != 0) return;
            _state.timer.Start();
            _state.gameSpeed = _state.prevGameSpeed;
            GameSpeedChanged?.Invoke(this, new EventArgs());
        }

        public void slowTime()
        {
            switch (_state.gameSpeed)
            {
                case 3:
                    _state.gameSpeed = 2;
                    _state.timer.Stop();
                    _state.timer.Interval = 500 / _state.gameSpeed;
                    _state.timer.Start();
                    break;
                case 2:
                    _state.gameSpeed = 1;
                    _state.timer.Stop();
                    _state.timer.Interval = 500 / _state.gameSpeed;
                    _state.timer.Start();
                    break;
                case 1:
                    _state.gameSpeed = 0;
                    _state.timer.Stop();                   
                    break;
            }
            GameSpeedChanged?.Invoke(this, new EventArgs());
            Debug.WriteLine("Gamespeed: " + GameSpeed);
        }

        public void speedUpTime()
        {
            switch (_state.gameSpeed)
            {
                case 0:
                    _state.gameSpeed = 1;
                    _state.timer.Stop();
                    _state.timer.Interval = 500 / _state.gameSpeed;
                    _state.timer.Start();
                    break;
                case 1:
                    _state.gameSpeed = 2;
                    _state.timer.Stop();
                    _state.timer.Interval = 500 / _state.gameSpeed;
                    _state.timer.Start();
                    break;
                case 2:
                    _state.gameSpeed = 3;
                    _state.timer.Stop();
                    _state.timer.Interval = 500 / _state.gameSpeed;
                    _state.timer.Start();
                    break;
            }
            GameSpeedChanged?.Invoke(this, new EventArgs());
            Debug.WriteLine("Gamespeed: " + GameSpeed);
        }

        private bool placeTile(Tile t, int x, int y)
        {
            if (_state.gameBoard[x, y] is Empty)
            {
                _state.gameBoard[x, y] = t;
                if (t is ResidentialZone)
                {
                    _state.residentalZones.Add((x, y));
                }
                else if (t is IndustrialZone)
                {
                    _state.industrialZones.Add((x, y));
                }
                else if (t is ServiceZone)
                {
                    _state.serviceZones.Add((x, y));
                }
                return true;
            }
            else
            {
                return false;
            }


        }

        public bool placeRoad(int x, int y)
        {
            Road r = new Road();
            _state.money = _state.money - Road.buildCost;
            _state.expense = _state.expense + Road.maintenanceFee;
            return placeTile(r, x, y);
        }

        public bool placeForest(int x, int y)
        {
            Forest f = new Forest(_state.time, false);
            _state.money -= Forest.BuildCost;
            _state.expense += Forest.MaintenanceFee;
            return placeTile(f, x, y);
        }

        public bool placeStadium(int x, int y)
        {
            Stadium s = new Stadium();
            _state.money = _state.money - Stadium.buildCost;
            _state.expense = _state.expense + Stadium.maintenanceFee;
            if (x + 1 < 50 && y + 1 < 50)
            {
                _state.gameBoard[x, y] = s;
                _state.gameBoard[x+1, y] = s;
                _state.gameBoard[x, y+1] = s;
                _state.gameBoard[x+1, y+1] = s;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool placePoliceStation(int x, int y)
        {
            PoliceStation p = new PoliceStation();
            _state.money = _state.money - PoliceStation.buildCost;
            _state.expense = _state.expense + PoliceStation.maintenanceFee;
            return placeTile(p, x, y);
        }

        public bool placeSchool(int x, int y)
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            School s = new School(id);
            _state.money = _state.money - School.buildCost;
            _state.expense = _state.expense + School.maintenanceFee;
            if (x + 1 < 50 && y + 1 < 50)
            {
                _state.gameBoard[x, y] = s;
                _state.gameBoard[x + 1, y] = s;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool placeUni(int x, int y)
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            University u = new University(id);
            _state.money = _state.money - University.buildCost;
            _state.expense = _state.expense + University.maintenanceFee;
            if (x + 1 < 50 && y + 1 < 50)
            {
                _state.gameBoard[x, y] = u;
                _state.gameBoard[x + 1, y] = u;
                _state.gameBoard[x, y + 1] = u;
                _state.gameBoard[x + 1, y + 1] = u;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool placeResidentialZone(int x, int y)
        {
            //Mennyi legyen a capacity?
            Debug.WriteLine("Placing residental in Model!");
            ResidentialZone r = new ResidentialZone(100);
            return placeTile(r, x, y);
        }

        public bool placeIndustrialZone(int x, int y)
        {
            //Mennyi legyen a capacity?
            Debug.WriteLine("Placing industrial in Model");
            IndustrialZone i = new IndustrialZone(100);
            return placeTile(i, x, y);
        }

        public bool placeServiceZone(int x, int y)
        {
            //Mennyi legyen a capacity?
            Debug.WriteLine("Placing service in Model");
            ServiceZone s = new ServiceZone(100);
            return placeTile(s, x, y);
        }

        public string getTileInfo()
        {
            return "";
        }

        public void destroyRoad(int x, int y)
        {
            if (_state.gameBoard[x, y] is Road && _state.canDestroyRoad(x, y))
                _state.gameBoard[x, y] = new Empty();
        }

        public bool destroyTile(Tile t)
        {
            if (t is Tile)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void catastrophe()
        {
            Random happens = new Random();
            int chance = happens.Next(0, 100);
            if (chance != 
                0)
            {
                int x;
                int y;
                x = happens.Next(0, 30);
                y = happens.Next(0, 30);
                _state.gameBoard[x, y] = new Empty();
                List<Tile> tiles = new List<Tile>();
                //tiles = getArea(5,x,y);

                //_state.gameBoard[x + 1, y] = new PoliceStation();

                int xflagP = 0;
                int xflagM = 0;
                if (30 - x < 5)
                {
                    for (int k = 0; k < 30 - x; ++k)
                    {
                        _state.gameBoard[x + k, y] = new Empty();
                        ++xflagP;
                    }
                }
                else
                {
                    for (int k = 0; k < 5; ++k)
                    {
                        _state.gameBoard[x + k, y] = new Empty();
                    }
                    xflagP = 5;
                }
                if (x - 5 < 0)
                {
                    for (int k = 0; k < x; ++k)
                    {
                        _state.gameBoard[x - k, y] = new Empty();
                        ++xflagM;
                    }
                }
                else
                {
                    for (int k = 0; k < 5; ++k)
                    {
                        _state.gameBoard[x - k, y] = new Empty();
                    }
                    xflagM = 5;
                }

                int yflagP = 0;
                int yflagM = 0;
                if (30 - y < 5)
                {
                    for (int k = 0; k < 30 - y; ++k)
                    {
                        _state.gameBoard[x, y + k] = new Empty();
                        ++yflagP;
                    }
                }
                else
                {
                    for (int k = 0; k < 5; ++k)
                    {
                        _state.gameBoard[x, y + k] = new Empty();
                    }
                    yflagP = 5;
                }
                if (y - 5 < 0)
                {
                    for (int k = 0; k < y; ++k)
                    {
                        _state.gameBoard[x, y - k] = new Empty();
                        ++yflagM;
                    }
                }
                else
                {
                    for (int k = 0; k < 5; ++k)
                    {
                        _state.gameBoard[x, y - k] = new Empty();
                    }
                    yflagM = 5;
                }

                //1:
                for (int k = 0; k < xflagP; ++k)
                {
                    for (int l = 0; l < yflagP; ++l)
                    {
                        _state.gameBoard[k, l] = new Empty();
                    }
                }

                //4:
                for (int k = 0; k < xflagP; ++k)
                {
                    for (int l = 0; l < yflagM; ++l)
                    {
                        _state.gameBoard[k, l] = new Empty();
                    }
                }

                //2:
                for (int k = 0; k < xflagM; ++k)
                {
                    for (int l = 0; l < yflagP; ++l)
                    {
                        _state.gameBoard[k, l] = new Empty();
                    }
                }

                //3:
                for (int k = 0; k < xflagM; ++k)
                {
                    for (int l = 0; l < yflagM; ++l)
                    {
                        _state.gameBoard[k, l] = new Empty();
                    }
                }

               // _state.gameBoard[x + 1, y] = new Stadium();



                catastropheHappened?.Invoke(this, new MaynotEventArg(x, y));
            }
        }  

        public void setResidentalTax(int tax)
        {
            if (tax > 0 && tax < 100) _state._residentalTax = tax;
            if ((_state._residentalTax == 0 && tax == 1) || (_state._residentalTax == 1 && tax == 0))  _state._residentalTax = tax; 
            if ((_state._residentalTax == 100 && tax == 99) || (_state._residentalTax == 99 && tax == 100))  _state._residentalTax = tax; 
        }
        public void setServiceTax(int tax)
        {
            if (tax > 0 && tax < 100) _state._serviceTax = tax;
            if ((_state._serviceTax == 0 && tax == 1) || (_state._serviceTax == 1 && tax == 0)) _state._serviceTax = tax;
            if ((_state._serviceTax == 100 && tax == 99) || (_state._serviceTax == 99 && tax == 100)) _state._serviceTax = tax;
        }
        public void setIndustrialTax(int tax)
        {
            if (tax > 0 && tax < 100) _state._industrialTax = tax;
            if ((_state._industrialTax == 0 && tax == 1) || (_state._industrialTax == 1 && tax == 0)) _state._industrialTax = tax;
            if ((_state._industrialTax == 100 && tax == 99) || (_state._industrialTax == 99 && tax == 100)) _state._industrialTax = tax;
        }
        private void taxCollection()
        {
            _state.money += _state.calculateIncome();
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _state);
        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _state = await _dataAccess.LoadAsync(path);
        }

        public void buildingEffects()
        {
            industrailZoneEffects();
            policeEffect();
            stadiumEffect();
            forestEffect();
        }

        private List<Tile> getArea(int size, int x, int y)
        {
            List<Tile> tiels = new List<Tile>();
            int xflagP = 0;
            int xflagM = 0;

            if (30 - x < 5)
            {
                for (int k = 0; k < 30 - x; ++k)
                {
                    tiels.Add(_state.gameBoard[x + k, y]);
                    ++xflagP;
                }
            }
            else
            {
                for (int k = 0; k < 5; ++k)
                {
                    tiels.Add(_state.gameBoard[x + k, y]);
                }
                xflagP = 5;
            }
            if (x - 5 < 0)
            {
                for (int k = 0; k < x; ++k)
                {
                    tiels.Add(_state.gameBoard[x - k, y]);
                    ++xflagM;
                }
            }
            else
            {
                for (int k = 0; k < 5; ++k)
                {
                    tiels.Add(_state.gameBoard[x - k, y]);
                }
                xflagM = 5;
            }

            int yflagP = 0;
            int yflagM = 0;
            if (30 - y < 5)
            {
                for (int k = 0; k < 30 - y; ++k)
                {
                    tiels.Add(_state.gameBoard[x, y + k]);
                    ++yflagP;
                }
            }
            else
            {
                for (int k = 0; k < 5; ++k)
                {
                    tiels.Add(_state.gameBoard[x, y + k]);
                }
                yflagP = 5;
            }
            if (y - 5 < 0)
            {
                for (int k = 0; k < y; ++k)
                {
                    tiels.Add(_state.gameBoard[x, y - k]);
                    ++yflagM;
                }
            }
            else
            {
                for (int k = 0; k < 5; ++k)
                {
                    tiels.Add(_state.gameBoard[x, y - k]);
                }
                yflagM = 5;
            }

            //1:
            for (int k = 0; k < xflagP; ++k)
            {
                for (int l = 0; l < yflagP; ++l)
                {
                    tiels.Add(_state.gameBoard[k, l]);
                }
            }

            //4:
            for (int k = 0; k < xflagP; ++k)
            {
                for (int l = 0; l < yflagM; ++l)
                {
                    tiels.Add(_state.gameBoard[k, l]);
                }
            }

            //2:
            for (int k = 0; k < xflagM; ++k)
            {
                for (int l = 0; l < yflagP; ++l)
                {
                    tiels.Add(_state.gameBoard[k, l]);
                }
            }

            //3:
            for (int k = 0; k < xflagM; ++k)
            {
                for (int l = 0; l < yflagM; ++l)
                {
                    tiels.Add(_state.gameBoard[k, l]);
                }
            }

            return tiels;
        }

        private void industrailZoneEffects()
        {
            List<Tile> tiels = new List<Tile>();
            for (int i = 0; i < _state.industrialZones.Count; ++i)
            {
                
                List<ResidentialZone> rez = new List<ResidentialZone>();
                int x = _state.industrialZones[i].Item1;
                int y = _state.industrialZones[i].Item2;
                tiels = getArea(5, x, y);
                for (int j = 0; j < tiels.Count; ++j)
                {
                    if (tiels[j] is MaynotPersistence.ResidentialZone)
                    {
                        rez.Add((ResidentialZone)tiels[j]);
                    }
                }
                for (int j = 0; j < rez.Count; ++j)
                {
                    for (int k = 0; k < rez[i].GetPeoples(_state.citizens).Count; ++k)
                    {

                        rez[j].GetPeoples(_state.citizens)[k].Satisfaction -= 5;
                        
                    }
                }
            }
        }

        private void policeEffect()
        {
            List<Tile> tiels = new List<Tile>();
            for (int i = 0; i < _state.size; ++i)
            {
                for (int k = 0; k < _state.size; ++k)
                {
                    if (_state.gameBoard[i, k] is PoliceStation)
                    {
                        List<ResidentialZone> rez = new List<ResidentialZone>();
                        int x = i;
                        int y = k;
                        tiels = getArea(5, x, y);
                        for (int j = 0; j < tiels.Count; ++j)
                        {
                            if (tiels[j] is MaynotPersistence.ResidentialZone)
                            {
                                rez.Add((ResidentialZone)tiels[j]);
                            }
                        }
                        for (int j = 0; j < rez.Count; ++j)
                        {
                            for (int l = 0; l < rez[j].GetPeoples(_state.citizens).Count; ++l)
                            {
                                //rez[i].People[j].Satisfaction = rez[i].People[j].Satisfaction + 5;
                                rez[j].GetPeoples(_state.citizens)[k].Satisfaction += 5;
                            }
                        }
                    }
                }             
            }
        }

        private void stadiumEffect()
        {
            List<Tile> tiels = new List<Tile>();
            for (int i = 0; i < _state.size; ++i)
            {
                for (int k = 0; k < _state.size; ++k)
                {
                    if (_state.gameBoard[i, k] is Stadium)
                    {
                        List<ResidentialZone> rez = new List<ResidentialZone>();
                        int x = i;
                        int y = k;
                        tiels = getArea(5, x, y);
                        for (int j = 0; j < tiels.Count; ++j)
                        {
                            if (tiels[j] is MaynotPersistence.ResidentialZone)
                            {
                                rez.Add((ResidentialZone)tiels[j]);
                            }
                        }
                        for (int j = 0; j < rez.Count; ++j)
                        {
                            for (int l = 0; l < rez[j].GetPeoples(_state.citizens).Count; ++l)
                            {
                                //rez[i].People[j].Satisfaction = rez[i].People[j].Satisfaction + 5;
                                rez[j].GetPeoples(_state.citizens)[k].Satisfaction += 5;
                            }
                        }
                    }
                }
            }
        }


        private void forestEffect()
        {
            List<Tile> tiels = new List<Tile>();
            for (int i = 0; i < _state.size; ++i)
            {
                for (int k = 0; k < _state.size; ++k)
                {
                    if (_state.gameBoard[i, k] is Forest)
                    {
                        List<ResidentialZone> rez = new List<ResidentialZone>();
                        int x = i;
                        int y = k;
                        tiels = getArea(5, x, y);
                        for (int j = 0; j < tiels.Count; ++j)
                        {
                            if (tiels[j] is MaynotPersistence.ResidentialZone)
                            {
                                rez.Add((ResidentialZone)tiels[j]);
                            }
                        }
                        for (int j = 0; j < rez.Count; ++j)
                        {
                            for (int l = 0; l < rez[j].GetPeoples(_state.citizens).Count; ++l)
                            {
                                //rez[i].People[j].Satisfaction = rez[i].People[j].Satisfaction + 5;
                                rez[j].GetPeoples(_state.citizens)[k].Satisfaction += 5;
                            }
                        }
                    }
                }
            }
        }

        private void growForests()
        {
            for (int i = 0; i < _state.size; ++i)
            {
                for (int k = 0; k < _state.size; ++k)
                {
                    if (_state.gameBoard[i, k] is Forest)
                    {
                        Forest? f = _state.gameBoard[i, k] as Forest;
                        if (f != null && (f.PlantingDate - Time).TotalDays % 365 == 0 && f.Age < 10)
                        {
                            f.Age++;
                            if(f.Age == 10 && !f.Generated)
                            {
                                _state.expense -= Forest.MaintenanceFee;
                            }
                        }
                    }
                }
            }
        }
        private void forestGenerator()
        {
            Random _r = new Random();
            int counter = _r.Next(5);
            for (int i = 0; i < counter; i++)
            {
                int x;
                int y;
                do
                {
                    x = _r.Next(GameBoardSize);
                    y = _r.Next(GameBoardSize);
                } while (GameBoard[x, y] is not Empty);

                GameBoard[x, y] = new Forest(Time, true);
                int area = _r.Next(1, 10);
                for (int k = x - area; k <= x + area; k++)
                {
                    for (int l = y - area; l <= y + area; l++)
                    {
                        if (_state.isValid(k, l) && GameBoard[k, l] is Empty)
                        {
                            int ap = _r.Next(area);
                            double distance = Math.Sqrt((x - k) * (x - k) + (y - l) * (y - l));
                            if (distance < area / 2)
                            {
                                GameBoard[k, l] = new Forest(Time, true);
                            }
                            else if(distance < ap)
                            {
                                GameBoard[k, l] = new Forest(Time, true);
                            }
                        }
                    }
                }
            }
        }
    }
}
