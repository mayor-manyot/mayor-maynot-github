using System;
using System.Collections.Generic;
using System.Windows.Markup.Localizer;
using MaynotPersistence;

namespace MaynotModel
{
    public class Model
    {
        private float money;
        private DateTime time;
        private int gameSpeed;
        private int prevGameSpeed;
        private Tile[,] gameBoard;
        private List<Person> citizens;
        private System.Timers.Timer timer;

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

        Model()
        {
            money = 0;
            gameSpeed = 0;
        }

        public void newGame()
        {
            money = 100000;
            gameSpeed = 1;
            time = new DateTime(0,0,0);
            citizens = new List<Person>();
            gameBoard = new Tile[50, 50];
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;

        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            time.AddDays(1);
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

        private void placeTile(Tile t)
        {
            

        }

        public void placeRoad()
        {
            Road r = new Road();
            placeTile(r);
        }

        public void placeForest()
        {
            Forest f = new Forest(time);
            placeTile(f);
        }


        public void placeResidentialZone()
        {
            //Mennyi legyen a capacity?
            ResidentialZone r = new ResidentialZone(100);
            placeTile(r);
        }

        public void placeIndustrialZone()
        {
            //Mennyi legyen a capacity?
            IndustrialZone i = new IndustrialZone(100);
            placeTile(i);
        }

        public void placeServiceZone()
        {
            //Mennyi legyen a capacity?
            ServiceZone s = new ServiceZone(100);
            placeTile(s);
        }

        public string getTileInfo()
        {
            return "";
        }

        public bool destroyTile(Tile t)
        {
            if (t == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void payServices() 
        { 
        
        }

        private void catastrophe()
        {

        }


    }
}
