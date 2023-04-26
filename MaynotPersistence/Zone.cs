using System;
using System.Collections.Generic;

namespace MaynotPersistence
{
    public interface Zone: Tile
    {
        public List<Person> People { get; set; }
        public Int32 Capacity { get; set; }
    }

    public class ResidentialZone: Zone
    {
        public List<Person> People { get; set; }
        public Int32 Capacity { get; set; }
        public ResidentialZone(Int32 capacity)
        {
            this.People = new List<Person>();
            this.Capacity = capacity;
        }

    }

    public class IndustrialZone : Zone
    {
        public List<Person> People { get; set; }
        public Int32 Capacity { get; set; }
        public IndustrialZone(Int32 capacity)
        {
            this.People = new List<Person>();
            this.Capacity = capacity;
        }
    }

    public class ServiceZone : Zone
    {
        public List<Person> People { get; set; }
        public Int32 Capacity { get; set; }
        public ServiceZone(Int32 capacity)
        {
            this.People = new List<Person>();
            this.Capacity = capacity;
        }
    }
}
