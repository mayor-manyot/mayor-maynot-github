using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class ResidentialZone : Zone
    {
        public ResidentialZone() : this(100, 0) { }
        public ResidentialZone(int capacity, int currentPopulation) : base(capacity, currentPopulation) { }
    }
}
