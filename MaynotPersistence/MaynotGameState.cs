using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace MaynotPersistence
{
    public class MaynotGameState
    {
        // 1/1 modell
        public float money;
        public DateTime time;
        public int gameSpeed;
        public int prevGameSpeed;
        public Tile[,] gameBoard;
        public List<Person> citizens;
        public System.Timers.Timer timer;
        public int yearTracker;
        public int weakTracker;
        public float income;
        public float expense;
        public List<(Zone, int, int)> homes;
        public List<(Zone, int, int)> workPlaces;

        //Konstruktor kapja meg a méretet és hozza létre az adattagokat null reference check miatt
        public MaynotGameState(int boardSize)
        {
            time = new DateTime(1, 1, 1);
            gameBoard = new Tile[boardSize, boardSize];
            timer = new System.Timers.Timer(500);
            citizens = new List<Person> ();
            homes = new List<(Zone, int, int)> ();
            workPlaces = new List<(Zone, int, int)> ();
        }
    }
}
