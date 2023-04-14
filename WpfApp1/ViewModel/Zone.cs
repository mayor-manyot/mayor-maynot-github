using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class Zone : MaynotTile
    {
        private Int32 _capacity;
        private Int32 _currentPopulation;
        public int CurrentPopulation { get { return _currentPopulation; } set { _currentPopulation = value; } }
        public Int32 Capacity { get { return _capacity; } set { _capacity = value; } }
        public ZoneType Type { get; set; }
    }

    public enum ZoneType
    {
        RESIDENTIAL,
        INDUSTRIAL,
        SERVICE,
        EMPTY,
    }
}
