using System;
using System.Collections.Generic;
using System.Linq;

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
        public bool guaranteedPopulation = true;

        public float expense;
        public float money;

        public List<Person> citizens;
        public List<(Zone, int, int)> homes;
        public List<(Zone, int, int)> workPlaces;

        public float _residentalTax;
        public float _industrialTax;
        public float _serviceTax;
        public double _averageSatisfaction;

        //Csak random adtam neki értékeket
        public const float _TaxMultElemLvl = 1.0F;
        public const float _TaxMultInterlLvl = 1.6F;
        public const float _TaxMulSuperLvl = 2.3F;

        public MaynotGameState(int boardSize)
        {
            timer = new System.Timers.Timer(500);
            time = new DateTime(1, 1, 1);

            gameBoard = new Tile[boardSize, boardSize];
            visited = new bool[boardSize, boardSize];

            size = boardSize;

            citizens = new List<Person> ();
            homes = new List<(Zone, int, int)>();
            workPlaces = new List<(Zone, int, int)>();

            setVisitedEmpty();
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
        public bool isRoad(int i, int j)
        {
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
        public bool path(int i, int j, int x, int y)
        {
            if (isValid(i, j) && isValid(x, y) && !visited[i, j])
            {
                visited[i, j] = true;

                if (isValid(i - 1, j) && i - 1 == x && j == y)
                {
                    return true;
                }
                else
                {
                    if (isRoad(i - 1, j))
                    {
                        bool up = path(i - 1, j, x, y);
                        if (up)
                            return true;
                    }
                }

                if (isValid(i, j - 1) && i == x && j - 1 == y)
                {
                    return true;
                }
                else
                {
                    if (isRoad(i, j - 1))
                    {
                        bool left = path(i, j - 1, x, y);
                        if (left)
                            return true;
                    }
                }

                if (isValid(i + 1, j) && i + 1 == x && j == y)
                {
                    return true;
                }
                else
                {
                    if (isRoad(i + 1, j))
                    {
                        bool down = path(i + 1, j, x, y);
                        if (down)
                            return true;
                    }
                }

                if (isValid(i, j + 1) && i == x && j + 1 == y)
                {
                    return true;
                }
                else
                {
                    if (isRoad(i, j + 1))
                    {
                        bool right = path(i, j + 1, x, y);
                        if (right)
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Eldönti hogy egy utat le lehet-e bontani.
        /// </summary>
        /// <returns>Igaz, ha le lehet bontani az utat, egyébként hamis.</returns>
        public bool canDestroyRoad(int i, int j)
        {
            if (getReachableBuildingsIntercept(i, j).Count == 0 || !isPath(14, 0, i, j))
                return true;

            return false;
        }

        /// <summary>
        /// Megnézi, hogy van-e út a két pont között.
        /// </summary>
        /// <returns>Igaz, ha van út a két pont között, egyébként hamis.</returns>
        public bool isPath(int i, int j, int x, int y)
        {
            setVisitedEmpty();
            return path(i, j, x, y);
        }
        public List<Tuple<int, int>> reachableBuildings(int i, int j, int x, int y)
        {
            List<Tuple<int, int>> reachableBuildingsList = new List<Tuple<int, int>>();
            if (isValid(i, j) && !visited[i, j])
            {
                visited[i, j] = true;

                if (isBuilding(i - 1, j))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i - 1, j));
                }
                else if (isRoad(i - 1, j))
                {
                    if (i - 1 != x || j != y)
                        reachableBuildingsList.AddRange(reachableBuildings(i - 1, j, x, y));
                }

                if (isBuilding(i, j - 1))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i, j - 1));
                }
                else if (isRoad(i, j - 1))
                {
                    if (i != x || j - 1 != y)
                        reachableBuildingsList.AddRange(reachableBuildings(i, j - 1, x, y));
                }

                if (isBuilding(i + 1, j))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i + 1, j));
                }
                else if (isRoad(i + 1, j))
                {
                    if (i + 1 != x || j != y)
                        reachableBuildingsList.AddRange(reachableBuildings(i + 1, j, x, y));
                }

                if (isBuilding(i, j + 1))
                {
                    reachableBuildingsList.Add(new Tuple<int, int>(i, j + 1));
                }
                else if (isRoad(i, j + 1))
                {
                    if (i != x || j + 1 != y)
                        reachableBuildingsList.AddRange(reachableBuildings(i, j + 1, x, y));
                }
            }

            return reachableBuildingsList;
        }

        /// <summary>
        /// Kilistázza az összes elérhető épületet a celláról.
        /// </summary>
        /// <returns>Épületek listája.</returns>
        public List<Tuple<int, int>> getReachableBuildings(int i, int j)
        {
            setVisitedEmpty();
            return reachableBuildings(i, j, size, size);
        }

        /// <summary>
        /// Kilistázza az összes elérhető épületet a start celláról egy útvonalat kivéve.
        /// </summary>
        /// <returns>Épületek listája.</returns>
        public List<Tuple<int, int>> getReachableBuildingsFromStart(int x, int y)
        {
            setVisitedEmpty();
            return reachableBuildings(14, 0, x, y);
        }

        /// <summary>
        /// Kilistázza az összes elérhető épületet a celláról, aminek nincs alternatív útvonala.
        /// </summary>
        /// <returns>Épületek listája.</returns>
        public List<Tuple<int, int>> getReachableBuildingsIntercept(int i, int j)
        {
            List<Tuple<int, int>> ReachableBuildingsFromCell = getReachableBuildings(i, j);
            List<Tuple<int, int>> ReachableBuildingsFromStart = getReachableBuildingsFromStart(i, j);
            ReachableBuildingsFromCell.RemoveAll(t => ReachableBuildingsFromStart.Contains(t));
            return ReachableBuildingsFromCell;
        }

        public float calculateAverageSatisfaction()
        {
            float averageSatisfaction = 0;
            foreach(Person p in citizens)
            {
                averageSatisfaction += p.Satisfaction;
            }

            return averageSatisfaction / citizens.Count;
        }
        public float calculateResidentalTax()
        {
            float residentalTax = 0;
            foreach(var zone in homes)
            {
                foreach(Person p in zone.Item1.People)
                {
                    residentalTax += _residentalTax;
                    //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                }
            }

            return residentalTax;
        }
        public float calculateWorkTax()
        {
            float workTax = 0;
            foreach (var zone in workPlaces)
            {
                foreach (Person p in zone.Item1.People)
                {
                    if (p.Education == Level.ELEMENTARY)
                    {
                        if(zone.Item1 is ServiceZone)
                        {
                            workTax += _serviceTax * _TaxMultElemLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                        else if(zone.Item1 is IndustrialZone)
                        {
                            workTax += _industrialTax * _TaxMultElemLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                    }

                    if (p.Education == Level.INTERMEDIATE)
                    {
                        if (zone.Item1 is ServiceZone)
                        {
                            workTax += _serviceTax * _TaxMultInterlLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                        else if (zone.Item1 is IndustrialZone)
                        {
                            workTax += _industrialTax * _TaxMultInterlLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                    }

                    if (p.Education == Level.SUPERLATIVE)
                    {
                        if (zone.Item1 is ServiceZone)
                        {
                            workTax += _serviceTax * _TaxMulSuperLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                        else if (zone.Item1 is IndustrialZone)
                        {
                            workTax += _industrialTax * _TaxMulSuperLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                    }
                }
            }

            return workTax;
        }
        public float calculateIncome()
        {
            float income = 0;
            income += calculateResidentalTax();
            income += calculateWorkTax();

            return income;
        }
    }
}
