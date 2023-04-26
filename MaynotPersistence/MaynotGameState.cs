using System;
using System.Collections.Generic;

namespace MaynotPersistence
{
    public class MaynotGameState
    {
        public System.Timers.Timer timer;
        public DateTime time;

        public Tile[,] gameBoard;
        public bool[,] visited;

        public int size;
        public int gameSpeed;
        public int prevGameSpeed;
        public int yearTracker;
        public int weakTracker;

        public float income;
        public float expense;
        public float money;

        public List<Person> citizens;
        public List<(Zone, int, int)> homes;
        public List<(Zone, int, int)> workPlaces;
        public List<(Zone, int, int)> serviceZones;

        public decimal _residentalTax;
        public decimal _industrialTax;
        public decimal _serviceTax;

        public MaynotGameState(int boardSize)
        {
            timer = new System.Timers.Timer(500);
            time = new DateTime(1, 1, 1);

            gameBoard = new Tile[boardSize, boardSize];
            visited = new bool[boardSize, boardSize];

            size = boardSize;

            citizens = new List<Person> ();
            homes = new List<(Zone, int, int)> ();
            workPlaces = new List<(Zone, int, int)> ();
            serviceZones = new List<(Zone, int, int)> ();

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    visited[i, j] = false;
        }

        public void setVisitedEmpty()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    visited[i, j] = false;
        }
        public bool isBuilding(int i, int j)
        {
            if (!isValid(i, j))
                return false;

            return gameBoard[i, j] is Zone || gameBoard[i, j] is Facility;
        }
        public bool isValid(int i, int j)
        {
            return i >= 0 && i < size && j >= 0 && j < size;
        }
        public bool isRoad(int i, int j, int startX, int startY)
        {
            if(isValid(i, j) && i == startX && j == startY) 
                return true;

            if(isValid(i, j))
                return gameBoard[i, j] is Road;

            return false;
        }
        public bool isDestination(int i, int j, int x, int y)
        {
            if(isValid(i, j))
                return i == x && j == y;

            return false;
        }
        public bool path(int i, int j, int x, int y, int startX, int startY)
        {
            if (isValid(i, j) && isValid(x, y) && isRoad(i, j, startX, startY) && !visited[i, j])
            {
                visited[i, j] = true;

                if (isValid(i - 1, j) && i - 1 == x && j == y)
                {
                    return true;
                }
                else
                {
                    bool up = path(i - 1, j, x, y, startX, startY);
                    if (up)
                        return true;
                }

                if (isValid(i, j - 1) && i == x && j - 1 == y)
                {
                    return true;
                }
                else
                {
                    bool left = path(i, j - 1, x, y, startX, startY);
                    if (left)
                        return true;
                }

                if (isValid(i + 1, j) && i + 1 == x && j == y)
                {
                    return true;
                }
                else
                {
                    bool down = path(i + 1, j, x, y, startX, startY);
                    if (down)
                        return true;
                }

                if (isValid(i, j + 1) && i == x && j + 1 == y)
                {
                    return true;
                }
                else
                {
                    bool right = path(i, j + 1, x, y, startX, startY);
                    if (right)
                        return true;
                }
            }

            return false;
        }
        public List<Tuple<int, int>> reachableBuildings(int i, int j, int startX, int startY)
        {
            List<Tuple<int, int>>? reachableBuildingsList = new List<Tuple<int, int>>();
            if (isValid(i, j) && isRoad(i, j, startX, startY) && !visited[i, j])
            {
                visited[i, j] = true;

                if (isValid(i - 1, j) && isBuilding(i - 1, j))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i - 1, j));
                }
                else
                {
                    reachableBuildingsList.AddRange(reachableBuildings(i - 1, j, startX, startY));
                }

                if (isValid(i, j - 1) && isBuilding(i, j - 1))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i, j - 1));
                }
                else
                {
                    reachableBuildingsList.AddRange(reachableBuildings(i, j - 1, startX, startY));
                }

                if (isValid(i + 1, j) && isBuilding(i + 1, j))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i + 1, j));
                }
                else
                {
                    reachableBuildingsList.AddRange(reachableBuildings(i + 1, j, startX, startY));
                }

                if (isValid(i, j + 1) && isBuilding(i, j + 1))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i, j + 1));
                }
                else
                {
                    reachableBuildingsList.AddRange(reachableBuildings(i, j + 1, startX, startY));
                }
            }
            return reachableBuildingsList;
        }

        /// <summary>
        /// Megnézi, hogy van-e út a két pont között.
        /// </summary>
        /// <returns>Igaz, ha van út a két pont között, egyébként hamis.</returns>
        public bool isPath(int i, int j, int x, int y)
        {
            setVisitedEmpty();
            return path(i, j, x, y, i, j);
        }

        /// <summary>
        /// Kilistázza az összes elérhető épületet a celláról.
        /// </summary>
        /// <returns>Épületek listája.</returns>
        public List<Tuple<int, int>> getReachableBuildings(int i, int j)
        {
            {
                setVisitedEmpty();
                return reachableBuildings(i, j, i, j);
            }
        }
    }
}
