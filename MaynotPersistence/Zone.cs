using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MaynotPersistence
{
    public enum ZoneLevel
    {
        SMALL = 1,
        MEDIUM = 2,
        LARGE = 3,
    }
    public interface Zone: Tile
    {
        //Csak az emberek indexeit tartalmazza
        public List<int> PeopleIndexes { get; set; }
        public ZoneLevel Level { get; set; }
        public int BuildCost { get; }
        public int UpgradeCost { get; }
        public Int32 Capacity { get; set; }
        public void Upgrade();

        //Vissza adja az emberek listáját
        public List<Person> GetPeoples(List<Person> citizens);
    }

    public class ResidentialZone: Zone
    {
        public ZoneLevel Level { get; set; }
        public int BuildCost { get; }
        public int UpgradeCost { get; }
        public List<int> PeopleIndexes { get; set; }
        public Int32 Capacity { get; set; }
        public ResidentialZone(Int32 capacity)
        {
            this.PeopleIndexes = new List<int>();
            this.Level = ZoneLevel.SMALL;
            this.BuildCost = 200;
            this.UpgradeCost = 300;
            this.Capacity = capacity;
        }

        public List<Person> GetPeoples(List<Person> citizens)
        {
            List<Person> zoneCitizens = new List<Person>();
            foreach (int index in PeopleIndexes)
            {
                Debug.WriteLine(citizens[index].Satisfaction);
                zoneCitizens.Add(citizens[index]);
            }
            return zoneCitizens;
        }
        public void Upgrade()
        {
            if (Level == ZoneLevel.SMALL)
            {
                Level = ZoneLevel.MEDIUM;
                Capacity *= 2;
            }
            else if (Level == ZoneLevel.MEDIUM)
            {
                Level = ZoneLevel.LARGE;
                Capacity *= 2;
            }
        }
    }

    public class IndustrialZone : Zone
    {
        public List<int> PeopleIndexes { get; set; }
        public ZoneLevel Level { get; set; }
        public int BuildCost { get; }
        public int UpgradeCost { get; }
        public Int32 Capacity { get; set; }
        public IndustrialZone(Int32 capacity)
        {
            this.PeopleIndexes = new List<int>();
            this.Level = ZoneLevel.SMALL;
            this.BuildCost = 200;
            this.UpgradeCost = 300;
            this.Capacity = capacity;
        }

        public List<Person> GetPeoples(List<Person> citizens)
        {
            List<Person> zoneCitizens = new List<Person>();
            foreach (int index in PeopleIndexes)
            {
                Debug.WriteLine(citizens[index].Satisfaction);
                zoneCitizens.Add(citizens[index]);
            }
            return zoneCitizens;
        }
        public void Upgrade()
        {
            if(Level == ZoneLevel.SMALL)
            {
                Level = ZoneLevel.MEDIUM;
                Capacity *= 2;
            }
            else if (Level == ZoneLevel.MEDIUM)
            {
                Level = ZoneLevel.LARGE;
                Capacity *= 2;
            }
        }
    }

    public class ServiceZone : Zone
    {
        public List<int> PeopleIndexes { get; set; }
        public ZoneLevel Level { get; set; }
        public int BuildCost { get; }
        public int UpgradeCost { get; }
        public Int32 Capacity { get; set; }
        public ServiceZone(Int32 capacity)
        {
            this.PeopleIndexes = new List<int>();
            this.Level = ZoneLevel.SMALL;
            this.BuildCost = 200;
            this.UpgradeCost = 300;
            this.Capacity = capacity;
        }

        public List<Person> GetPeoples(List<Person> citizens)
        {
            List<Person> zoneCitizens = new List<Person>();
            foreach (int index in PeopleIndexes)
            {
                Debug.WriteLine(citizens[index].Satisfaction);
                zoneCitizens.Add(citizens[index]);
            }
            return zoneCitizens;
        }
        public void Upgrade()
        {
            if (Level == ZoneLevel.SMALL)
            {
                Level = ZoneLevel.MEDIUM;
                Capacity *= 2;
            }
            else if (Level == ZoneLevel.MEDIUM)
            {
                Level = ZoneLevel.LARGE;
                Capacity *= 2;
            }
        }
    }
}
