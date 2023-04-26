using System;
using System.Collections.Generic;
using System.Diagnostics;
using MaynotPersistence;

namespace MaynotModel
{
    public class MaynotGameModel
    {
        private MaynotGameState _state;
        
        public event EventHandler<MaynotEventArg>? SaveGame;
        public event EventHandler<MaynotEventArg>? LoadGame;
        public event EventHandler<MaynotEventArg>? ExitGame;
        public event EventHandler<MaynotEventArg>? TilePlaced;
        public event EventHandler<MaynotEventArg>? GeneralInfoAsked;
        public event EventHandler<MaynotEventArg>? catastropheHappened;

        public float Money { get => _state.money; set => _state.money = value; }
        public DateTime Time { get => _state.time; set => _state.time = value; }
        public int GameSpeed { get => _state.gameSpeed; set => _state.gameSpeed = value; }
        public Tile[,] GameBoard { get => _state.gameBoard; set => _state.gameBoard = value; }
        public int GameBoardSize { get => _state.size; }
        public List<Person> Citizens { get => _state.citizens; set => _state.citizens = value; }

        public MaynotGameModel()
        {
            _state = new MaynotGameState(0);
        }

        public void newGame()
        {
            _state = new MaynotGameState(30);
            _state.money = 100000;
            _state.gameSpeed = 1;
            _state.yearTracker = 1;
            _state.weakTracker = 0;
            //azt van használva a lehelyezésnél
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    _state.gameBoard[i, j] = new Empty();
                }
            }
            _state.gameBoard[14, 0] = new Road();

            _state.timer.Elapsed += Timer_Elapsed;            
            Debug.WriteLine("New game");
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _state.time.AddDays(1);
            _state.weakTracker++;
            if (_state.yearTracker != _state.time.Year)
            {
                _state.yearTracker = _state.time.Month;
                _state.money = _state.money - _state.expense;
                _state.money = _state.money + _state.income;
            }
            movingIn();
        }

        private void movingIn()
        {
            if (_state.weakTracker % 7 == 0)
            {
                Random r = new Random();
                int num = r.Next(1, 25);
                if (_state.homes.Count == 0 || _state.workPlaces.Count == 0) { }
                else
                {
                    for (int i = 0; i < num; ++i)
                    {
                        //Kikommenteltem warning miatt
                        //Zone home;
                        int n = r.Next(0, 99);
                        if (n < 18)
                        {
                            int k = 0;
                            for (int j = 0;j < _state.homes.Count; ++j) 
                            {
                                if (_state.homes[j].Item1.Capacity < _state.homes[k].Item1.Capacity
                                    && _state.isPath(GameBoardSize / 2 - 1, 0, _state.homes[k].Item2, _state.homes[k].Item3))
                                {
                                    k = j;
                                }
                            }
                            Person p = new Person(50, n, _state.homes[k].Item1, null, Level.ELEMENTARY);
                            _state.citizens.Add(p);
                        }
                        else
                        {
                            int k = 0;
                            for (int j = 0; j < _state.homes.Count; ++j)
                            {
                                if (_state.homes[j].Item1.Capacity < _state.homes[k].Item1.Capacity)
                                {
                                    k = j;
                                }
                            }
                            for (int j = 0; j < _state.workPlaces.Count; ++j)
                            {
                                if (_state.workPlaces[j].Item1.Capacity < _state.workPlaces[k].Item1.Capacity)
                                {
                                    k = j;
                                }
                            }
                            int ch = r.Next(1,3);
                            Person p;
                            if (ch == 2) 
                            {
                                p = new Person(50, n, _state.workPlaces[k].Item1, null, Level.INTERMEDIATE);
                            }
                            else
                            {
                                p = new Person(50, n, _state.workPlaces[k].Item1, null, Level.SUPERLATIVE);
                            }
                            _state.citizens.Add(p);

                        }

                    }
                }
            }
        }

        public void stopTime()
        {
            _state.timer.Stop();
            _state.prevGameSpeed = _state.gameSpeed;
            _state.gameSpeed = 0;
        }

        public void resumeTime()
        {
            _state.timer.Start();
            _state.gameSpeed = _state.prevGameSpeed;
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
            Debug.WriteLine("Gamespeed: " + GameSpeed);
        }

        private bool placeTile(Tile t, int x, int y)
        {
            if (_state.gameBoard[x, y] is Empty)
            {
                _state.gameBoard[x, y] = t;
                if (t is ResidentialZone)
                {
                    (Tile, int, int) adding = (t, x, y);
                    _state.homes.Add(((Zone, int, int))adding);
                }else if(t is IndustrialZone)
                {
                    (Tile, int, int) adding = (t, x, y);
                    _state.homes.Add(((Zone, int, int))adding);
                }
                else if (t is ServiceZone)
                {
                    (Tile, int, int) adding = (t, x, y);
                    _state.homes.Add(((Zone, int, int))adding);
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
            Forest f = new Forest(_state.time);
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
            IndustrialZone i = new IndustrialZone(100);
            return placeTile(i, x, y);
        }

        public bool placeServiceZone(int x, int y)
        {
            //Mennyi legyen a capacity?
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
            if (chance == 0)
            {
                int x;
                int y;
                x = happens.Next(0, 30);
                y = happens.Next(0, 30);
                _state.gameBoard[x, y] = new Empty();
                int xdif;
                xdif = 30 - x;
                int ydif;
                ydif = 30 - y;
                int xflag = 0;
                for (int i = 0; i < xdif;  ++i)
                {
                    xflag++;
                    _state.gameBoard[x-i, y] = new Empty();
                    if (xflag == 5)
                    {
                        break;
                    }
                }
                if (xflag < xdif)
                {
                    xflag = xdif;
                }

                int yflag = 0;
                for (int i = 0; i < ydif; ++i)
                {
                    yflag++;
                    _state.gameBoard[x, y - i] = new Empty();
                    if (yflag == 5)
                    {
                        break;
                    }
                }
                if (yflag < ydif)
                {
                    yflag = ydif;
                }
                for (int j = -yflag; j < yflag; ++j)
                {
                    for (int i = -xflag; i < xflag; ++i)
                    {
                        _state.gameBoard[x + i, y + j] = new Empty();
                    }
                }
                

                //catastropheHappened?.Invoke(this, new MaynotEventArg(x, y));
            }
        }  
    }
}
