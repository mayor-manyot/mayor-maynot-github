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
        public float income;
        public float expense;
        // + amit majd menteni kell

        public MaynotGameState(float money, DateTime time, int gameSpeed, int prevGameSpeed, Tile[,] gameBoard, List<Person> citizens, Timer timer, int yearTracker, float income, float expense)
        {
            this.money = money;
            this.time = time;
            this.gameSpeed = gameSpeed;
            this.prevGameSpeed = prevGameSpeed;
            this.gameBoard = gameBoard;
            this.citizens = citizens;
            this.timer = timer;
            this.yearTracker = yearTracker;
            this.income = income;
            this.expense = expense;
        }
    }
}
