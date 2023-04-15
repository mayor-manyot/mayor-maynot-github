using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class Stadium : Facility
    {
        public Stadium() : this(100, 0, 5000) { }
        public Stadium(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost) { }
    }
}
