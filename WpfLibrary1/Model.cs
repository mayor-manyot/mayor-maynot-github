using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Markup.Localizer;
using MaynotPersistence;

namespace MaynotModel
{
    public class MaynotGameModel
    {
        private float money;
        private DateTime time;
        private int gameSpeed;
        private int prevGameSpeed;
        private Tile[,] gameBoard;
        private List<Person> citizens;
        private System.Timers.Timer timer;
        private int yearTracker;
        private int weakTracker;
        private float income;
        private float expense;
        private List<(Zone, int, int)> homes;
        private List<(Zone, int, int)> workPlaces;

        public event EventHandler<MaynotEventArg> SaveGame;
        public event EventHandler<MaynotEventArg> LoadGame;
        public event EventHandler<MaynotEventArg> ExitGame;
        public event EventHandler<MaynotEventArg> TilePlaced;
        public event EventHandler<MaynotEventArg> GeneralInfoAsked;

        public float Money { get => money; set => money = value; }
        public DateTime Time { get => time; set => time = value; }
        public int GameSpeed { get => gameSpeed; set => gameSpeed = value; }
        public Tile[,] GameBoard { get => gameBoard; set => gameBoard = value; }
        public List<Person> Citizens { get => citizens; set => citizens = value; }

        public MaynotGameModel()
        {
            money = 0;
            gameSpeed = 0;
        }

        public void newGame()
        {
            money = 100000;
            gameSpeed = 1;
            time = new DateTime(1,1,1);
            yearTracker = 1;
            weakTracker = 0;
            citizens = new List<Person>();
            //azt van használva a lehelyezésnél
            gameBoard = new Tile[30, 30];
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    gameBoard[i, j] = new Empty();
                }
            }
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;
            homes = new List<(Zone, int, int)>();
            workPlaces = new List<(Zone, int, int)>();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            time.AddDays(1);
            weakTracker++;
            if (yearTracker != time.Year)
            {
                yearTracker = time.Month;
                money = money - expense;
                money = money + income;
            }
            movingIn();
        }

        private void movingIn()
        {
            if (weakTracker % 7 == 0)
            {
                Random r = new Random();
                int num = r.Next(1, 25);
                if (homes.Count == 0 || workPlaces.Count == 0) { }
                else
                {
                    for (int i = 0; i < num; ++i)
                    {
                        Zone home;
                        int n = r.Next(0, 99);
                        if (n < 18)
                        {
                            int k = 0;
                            for (int j = 0;j < homes.Count; ++j) 
                            {
                                if (homes[j].Item1.Capacity < homes[k].Item1.Capacity)
                                {
                                    k = j;
                                }
                            }
                            Person p = new Person(50, n, homes[k].Item1, null, Level.ELEMENTARY);
                            citizens.Add(p);
                        }
                        else
                        {
                            int k = 0;
                            for (int j = 0; j < homes.Count; ++j)
                            {
                                if (homes[j].Item1.Capacity < homes[k].Item1.Capacity)
                                {
                                    k = j;
                                }
                            }
                            for (int j = 0; j < workPlaces.Count; ++j)
                            {
                                if (workPlaces[j].Item1.Capacity < workPlaces[k].Item1.Capacity)
                                {
                                    k = j;
                                }
                            }
                            int ch = r.Next(1,3);
                            Person p;
                            if (ch == 2) 
                            {
                                p = new Person(50, n, workPlaces[k].Item1, null, Level.INTERMEDIATE);
                            }
                            else
                            {
                                p = new Person(50, n, workPlaces[k].Item1, null, Level.SUPERLATIVE);
                            }
                            citizens.Add(p);

                        }

                    }
                }

                
                
                
            }
        }

        public void stopTime()
        {
            timer.Stop();
            prevGameSpeed = gameSpeed;
            gameSpeed = 0;
        }

        public void resumeTime()
        {
            timer.Start();
            gameSpeed = prevGameSpeed;
        }

        public void slowTime()
        {
            switch (gameSpeed)
            {
                case 3:
                    gameSpeed = 2;
                    timer.Stop();
                    timer.Interval = 500 / gameSpeed;
                    timer.Start();
                    break;
                case 2:
                    gameSpeed = 1;
                    timer.Stop();
                    timer.Interval = 500 / gameSpeed;
                    timer.Start();
                    break;
                case 1:
                    gameSpeed = 0;
                    timer.Stop();                   
                    break;
            }
            Debug.WriteLine("Gamespeed: " + GameSpeed);
        }

        public void speedUpTime()
        {
            switch (gameSpeed)
            {
                case 0:
                    gameSpeed = 1;
                    timer.Stop();
                    timer.Interval = 500 / gameSpeed;
                    timer.Start();
                    break;
                case 1:
                    gameSpeed = 2;
                    timer.Stop();
                    timer.Interval = 500 / gameSpeed;
                    timer.Start();
                    break;
                case 2:
                    gameSpeed = 3;
                    timer.Stop();
                    timer.Interval = 500 / gameSpeed;
                    timer.Start();
                    break;
            }
            Debug.WriteLine("Gamespeed: " + GameSpeed);
        }

        private bool placeTile(Tile t, int x, int y)
        {
            if (gameBoard[x, y] is Tile)
            {
                gameBoard[x, y] = t;
                if (t is ResidentialZone)
                {

                    (Tile, int, int) adding = (t, x, y);
                    homes.Add(((Zone, int, int))adding);
                }else if(t is IndustrialZone)
                {
                    (Tile, int, int) adding = (t, x, y);
                    homes.Add(((Zone, int, int))adding);
                }
                else if (t is ServiceZone)
                {
                    (Tile, int, int) adding = (t, x, y);
                    homes.Add(((Zone, int, int))adding);
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
            money = money - Road.buildCost;
            expense = expense + Road.maintenanceFee;
            return placeTile(r, x, y);
        }

        public bool placeForest(int x, int y)
        {
            Forest f = new Forest(time);
            return placeTile(f, x, y);
        }

        public bool placeStadium(int x, int y)
        {
            Stadium s = new Stadium();
            money = money - Stadium.buildCost;
            expense = expense + Stadium.maintenanceFee;
            if (x + 1 < 50 && y + 1 < 50)
            {
                gameBoard[x, y] = s;
                gameBoard[x+1, y] = s;
                gameBoard[x, y+1] = s;
                gameBoard[x+1, y+1] = s;
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
            money = money - PoliceStation.buildCost;
            expense = expense + PoliceStation.maintenanceFee;
            return placeTile(p, x, y);
        }

        public bool placeSchool(int x, int y)
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            School s = new School(id);
            money = money - School.buildCost;
            expense = expense + School.maintenanceFee;
            if (x + 1 < 50 && y + 1 < 50)
            {
                gameBoard[x, y] = s;
                gameBoard[x + 1, y] = s;
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
            money = money - University.buildCost;
            expense = expense + University.maintenanceFee;
            if (x + 1 < 50 && y + 1 < 50)
            {
                gameBoard[x, y] = u;
                gameBoard[x + 1, y] = u;
                gameBoard[x, y + 1] = u;
                gameBoard[x + 1, y + 1] = u;
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

        }


    }
}
