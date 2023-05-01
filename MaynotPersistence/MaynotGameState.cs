using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace MaynotPersistence
{
    public class MaynotGameState
    {
        public System.Timers.Timer timer;
        public DateTime time;

        public Tile[,] gameBoard;
        public bool[,] visited;

        public int size;
        public int startX;
        public int startY;
        public int gameSpeed;
        public int prevGameSpeed;
        public int yearTracker;
        public int weakTracker;
        public bool guaranteedPopulation = false;

        public float income;
        public float expense;
        public float money;

        public List<Person> citizens;
        //Csak a koordinátákat tartalmazza
        public List<(int, int)> residentalZones;
        public List<(int, int)> serviceZones;
        public List<(int, int)> industrialZones;

        public int _residentalTax;
        public int _industrialTax;
        public int _serviceTax;
        public double _averageSatisfaction;

        //Csak random adtam neki értékeket
        private const float _TaxMultElemLvl = 1.0F;
        private const float _TaxMultInterlLvl = 1.6F;
        private const float _TaxMulSuperLvl = 2.3F;

        public MaynotGameState(int boardSize)
        {
            timer = new System.Timers.Timer(500);
            time = new DateTime(1, 1, 1);

            gameBoard = new Tile[boardSize, boardSize];
            visited = new bool[boardSize, boardSize];

            size = boardSize;

            citizens = new List<Person> ();
            residentalZones = new List<(int, int)>();
            serviceZones = new List<(int, int)>();
            industrialZones = new List<(int, int)>();

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
        public int distance(int i, int j, int x, int y)
        {
            if (isValid(i, j) && isValid(x, y) && !visited[i, j])
            {
                visited[i, j] = true;

                if (isValid(i - 1, j) && i - 1 == x && j == y)
                {
                    return 1;
                }
                else
                {
                    if (isRoad(i - 1, j))
                    {
                        int up = distance(i - 1, j, x, y);
                        if (up > 0)
                            return up + 1;
                    }
                }

                if (isValid(i, j - 1) && i == x && j - 1 == y)
                {
                    return 1;
                }
                else
                {
                    if (isRoad(i, j - 1))
                    {
                        int left = distance(i, j - 1, x, y);
                        if (left > 0)
                            return left + 1;
                    }
                }

                if (isValid(i + 1, j) && i + 1 == x && j == y)
                {
                    return 1;
                }
                else
                {
                    if (isRoad(i + 1, j))
                    {
                        int down = distance(i + 1, j, x, y);
                        if (down > 0)
                            return down + 1;
                    }
                }

                if (isValid(i, j + 1) && i == x && j + 1 == y)
                {
                    return 1;
                }
                else
                {
                    if (isRoad(i, j + 1))
                    {
                        int right = distance(i, j + 1, x, y);
                        if (right > 0)
                            return right + 1;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Eldönti hogy egy utat le lehet-e bontani.
        /// </summary>
        /// <returns>Igaz, ha le lehet bontani az utat, egyébként hamis.</returns>
        public bool canDestroyRoad(int i, int j)
        {
            if (getReachableBuildingsIntercept(i, j).Count == 0 || isDistance(14, 0, i, j) > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Megnézi, hogy van-e út a két pont között.
        /// </summary>
        /// <returns>Igaz, ha van út a két pont között, egyébként hamis.</returns>
        public int isDistance(int i, int j, int x, int y)
        {
            setVisitedEmpty();
            return distance(i, j, x, y);
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
            foreach ((int x, int y) in residentalZones)
            {
                Zone? z = gameBoard[x, y] as Zone;
                if (z != null)
                {
                    foreach (Person p in z.GetPeoples(citizens))
                    {
                        residentalTax += _residentalTax;
                        //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                    }
                }
            }

            return residentalTax;
        }
        public float calculateServiceTax()
        {
            float serviceTax = 0;
            foreach ((int x, int y) in serviceZones)
            {
                Zone? z = gameBoard[x, y] as Zone;
                if (z != null)
                {
                    foreach (Person p in z.GetPeoples(citizens))
                    {
                        if (p.Education == Level.ELEMENTARY)
                        {
                            serviceTax += _serviceTax * _TaxMultElemLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }

                        if (p.Education == Level.INTERMEDIATE)
                        {
                            serviceTax += _serviceTax * _TaxMultInterlLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }

                        if (p.Education == Level.SUPERLATIVE)
                        {
                            serviceTax += _serviceTax * _TaxMulSuperLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                    }
                }
            }

            return serviceTax;
        }
        public float calculateIndustrialTax()
        {
            float industrialTax = 0;
            foreach ((int x, int y) in industrialZones)
            {
                Zone? z = gameBoard[x, y] as Zone;
                if (z != null)
                {
                    foreach (Person p in z.GetPeoples(citizens))
                    {
                        if (p.Education == Level.ELEMENTARY)
                        {
                            industrialTax += _industrialTax * _TaxMultElemLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }

                        if (p.Education == Level.INTERMEDIATE)
                        {
                            industrialTax += _industrialTax * _TaxMultInterlLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }

                        if (p.Education == Level.SUPERLATIVE)
                        {
                            industrialTax += _industrialTax * _TaxMulSuperLvl;
                            //TODO: befizetett adó tárolása személyeknél a nyugdíj miatt
                        }
                    }
                }
            }

            return industrialTax;
        }
        public float calculateIncome()
        {
            float income = 0;
            income += calculateResidentalTax();
            income += calculateServiceTax();
            income += calculateIndustrialTax();

            return income;
        }
        public double RatioIndustrialZonesFullness()
        {
            int counterOccupied = 0;
            int counterAll = 0;
            foreach ((int x, int y) in industrialZones)
            {
                if (gameBoard[x, y] is Zone z)
                {
                    counterOccupied += z.PeopleIndexes.Count;
                    counterAll += z.Capacity;
                }
            }

            if (counterAll != 0)
            {
                return (double)counterOccupied / counterAll;
            }

            return 0;
        }
        public double RatioServiceZonesFullness()
        {
            int counterOccupied = 0;
            int counterAll = 0;
            foreach ((int x, int y) in serviceZones)
            {
                if (gameBoard[x, y] is Zone z)
                {
                    counterOccupied += z.PeopleIndexes.Count;
                    counterAll += z.Capacity;
                }
            }

            if (counterAll != 0)
            {
                return (double)counterOccupied / counterAll;
            }

            return 0;
        }
        public (int x, int y) getFreeResidentalZone()
        {
            Random r = new Random();
            List<(int x, int y)> freeResidentialZones = new List<(int x, int y)>();
            foreach ((int x, int y) in residentalZones)
            {
                if (gameBoard[x, y] is Zone z && z.Capacity > z.PeopleIndexes.Count && isDistance(x, y, startX, startY) > 0)
                {
                    freeResidentialZones.Add((x, y));
                }
            }

            if (freeResidentialZones.Count > 0)
            {
                int randomIndex = r.Next(0, freeResidentialZones.Count);
                return freeResidentialZones[randomIndex];
            }

            return (-1, -1);
        }
        public (int x, int y) getFreeServiceZone(int i, int j)
        {
            Random r = new Random();
            List<(int x, int y, int d)> freeServiceZones = new List<(int x, int y, int d)>();
            foreach ((int x, int y) in serviceZones)
            {
                if (gameBoard[x, y] is Zone z && z.Capacity > z.PeopleIndexes.Count && isDistance(x, y, i, j) > 0)
                {
                    freeServiceZones.Add((x, y, isDistance(x, y, i, j)));
                }
            }

            if (freeServiceZones.Count > 0)
            {
                freeServiceZones.Sort((a, b) => a.d.CompareTo(b.d));
                return (freeServiceZones[0].x, freeServiceZones[0].y);
            }

            return (-1, -1);
        }
        public (int x, int y) getFreeIndustrialZone(int i, int j)
        {
            Random r = new Random();
            List<(int x, int y, int d)> freeIndustrialZones = new List<(int x, int y, int d)>();
            foreach ((int x, int y) in industrialZones)
            {
                if (gameBoard[x, y] is Zone z && z.Capacity > z.PeopleIndexes.Count && isDistance(x, y, i, j) > 0)
                {
                    freeIndustrialZones.Add((x, y, isDistance(x, y, i, j)));
                }
            }
            
            if (freeIndustrialZones.Count > 0)
            {
                freeIndustrialZones.Sort((a, b) => a.d.CompareTo(b.d));
                return (freeIndustrialZones[0].x, freeIndustrialZones[0].y);
            }

            return (-1, -1);
        }
        public (int x, int y) getFreeWorkZone(int i, int j)
        {
            (int x, int y) workPlace;
            if (RatioServiceZonesFullness() < 1 && RatioIndustrialZonesFullness() == 1)
            {
                workPlace = getFreeServiceZone(i, j);
            }
            else if (RatioServiceZonesFullness() == 1 && RatioIndustrialZonesFullness() < 1)
            {
                workPlace = getFreeIndustrialZone(i, j);
            }
            else if (RatioServiceZonesFullness() < RatioIndustrialZonesFullness())
            {
                workPlace = getFreeServiceZone(i, j);
            }
            else
            {
                workPlace = getFreeIndustrialZone(i, j);
            }

            return workPlace;
        }
        public int findClosestIndustrialZone(int i, int j)
        {
            int value = 0;
            int circleSize = 4;
            (int, int)[] direction = { (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1) };

            int l = 0;
            int x = i;
            int y = j;
            while (l < circleSize && !(gameBoard[x, y] is IndustrialZone))
            {
                int k = 0;
                while (k < direction.Length && !(gameBoard[x, y] is IndustrialZone))
                {
                    x = i + direction[k].Item1 * l;
                    y = j + direction[k].Item2 * l;
                    k++;
                }
                l++;
            }

            if (gameBoard[x, y] is IndustrialZone)
            {
                value = l;
            }

            return value;
        }
        public int calculateZoneAttractiveness(Person p)
        {
            int attractiveness = 100;
            attractiveness -= (int)calculateAverageSatisfaction();
            attractiveness += isDistance(p.Residency.Item1, p.Residency.Item2, p.WorkPlace.Item1, p.WorkPlace.Item2);
            attractiveness += (int)Math.Pow(4, findClosestIndustrialZone(p.Residency.Item1, p.Residency.Item2));
            return attractiveness;
        }
    }
}
