using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
        public override string DisplayName { get; } = "Facility";
        public Facility(FacilityType facilityType) : this(10, 0, facilityType, 5000) { }
        public Facility(int capacity, int currentPopulation, FacilityType type, int buildCost)
        {
            Capacity = capacity;
            CurrentPopulation = currentPopulation;
            Type = type;
            BuildCost = buildCost;
            switch (type)
            {
                case FacilityType.POLICESTATION:
                    Background = new SolidColorBrush(Colors.Blue);
                    break;
                case FacilityType.STADIUM:
                    Background = new SolidColorBrush(Colors.Orange);
                    break;
                case FacilityType.SCHOOL:
                    Background = new SolidColorBrush(Colors.MediumVioletRed);
                    break;
                case FacilityType.UNIVERSITY:
                    Background = new SolidColorBrush(Colors.RoyalBlue);
                    break;
                default:
                    break;
            }
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
