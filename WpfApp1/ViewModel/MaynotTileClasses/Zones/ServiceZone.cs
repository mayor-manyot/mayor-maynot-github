using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class ServiceZone : Zone
    {
        public override string DisplayName { get; } = "Szolgáltatás Zóna";
        public ServiceZone() : this(100, 0) { }
        public ServiceZone(int capacity, int currentPopulation) : base(capacity, currentPopulation) { }
    }
}
