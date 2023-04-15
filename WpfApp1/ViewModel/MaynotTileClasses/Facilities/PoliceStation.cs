using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class PoliceStation : Facility
    {
        public PoliceStation() : this(100, 0, 5000) { }
        public PoliceStation(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost) { }
    }
}
