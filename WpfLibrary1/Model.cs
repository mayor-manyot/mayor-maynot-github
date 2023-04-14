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
        private float income;
        private float expense;

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
            citizens = new List<Person>();
            //azt van használva a lehelyezésnél
            gameBoard = new Tile[30, 30];
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;
            Debug.WriteLine("New game");

        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            time.AddDays(1);
            if (yearTracker != time.Year)
            {
                yearTracker = time.Month;
                money = money - expense;
                money = money + income;
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
        }

        private bool placeTile(Tile t, int x, int y)
        {
            if (gameBoard[x, y] is Tile)
            {
                gameBoard[x, y] = t;
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
