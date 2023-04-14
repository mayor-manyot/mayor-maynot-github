using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class Facility : MaynotTile 
    {
        private Int32 _capacity;
        private Int32 _currentPopulation;
        private FacilityType _type;
        public int CurrentPopulation { get { return _currentPopulation; } set { _currentPopulation = value; } }
        public Int32 Capacity { get { return _capacity; } set { _capacity = value; } }
        public FacilityType Type { get { return _type; } set { _type = value; } }
        public Int32 BuildCost { get; set; }
        public Facility(int capacity, int currentPopulation, FacilityType type, int buildCost)
        {
            Capacity = capacity;
            CurrentPopulation = currentPopulation;
            Type = type;
            BuildCost = buildCost;
        }
    }

    public enum FacilityType
    {
        POLICESTATION,
        STADIUM,
        SCHOOL,
        UNIVERSITY
    }
}
