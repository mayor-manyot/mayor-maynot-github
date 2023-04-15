using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class University : Facility
    {
        public University() : this(100, 0, 5000) { }
        public University(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost) { }
    }
}
