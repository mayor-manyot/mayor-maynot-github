using System;
using System.Collections.Generic;

namespace MaynotPersistence
{
    public interface Zone: Tile
    {
        //Csak az emberek indexeit tartalmazza
        public List<int> PeopleIndexes { get; set; }
        public Int32 Capacity { get; set; }

        //Vissza adja az emberek listáját
        public List<Person> GetPeoples(List<Person> citizens);
    }

    public class ResidentialZone: Zone
    {
        public List<int> PeopleIndexes { get; set; }
        public Int32 Capacity { get; set; }
        public ResidentialZone(Int32 capacity)
        {
            this.PeopleIndexes = new List<int>();
            this.Capacity = capacity;
        }

        public List<Person> GetPeoples(List<Person> citizens)
        {
            List<Person> zoneCitizens = new List<Person>();
            foreach (int index in PeopleIndexes)
            {
                zoneCitizens.Add(citizens[index]);
            }
            return zoneCitizens;
        }
    }

    public class IndustrialZone : Zone
    {
        public List<int> PeopleIndexes { get; set; }
        public Int32 Capacity { get; set; }
        public IndustrialZone(Int32 capacity)
        {
            this.PeopleIndexes = new List<int>();
            this.Capacity = capacity;
        }

        public List<Person> GetPeoples(List<Person> citizens)
        {
            List<Person> zoneCitizens = new List<Person>();
            foreach (int index in PeopleIndexes)
            {
                zoneCitizens.Add(citizens[index]);
            }
            return zoneCitizens;
        }
    }

    public class ServiceZone : Zone
    {
        public List<int> PeopleIndexes { get; set; }
        public Int32 Capacity { get; set; }
        public ServiceZone(Int32 capacity)
        {
            this.PeopleIndexes = new List<int>();
            this.Capacity = capacity;
        }

        public List<Person> GetPeoples(List<Person> citizens)
        {
            List<Person> zoneCitizens = new List<Person>();
            foreach (int index in PeopleIndexes)
            {
                zoneCitizens.Add(citizens[index]);
            }
            return zoneCitizens;
        }
    }
}
