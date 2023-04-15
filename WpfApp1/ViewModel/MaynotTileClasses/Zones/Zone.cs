using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class Zone : MaynotTile
    {
        private Int32 _capacity;
        private Int32 _currentPopulation;

        public override string DisplayName { get; } = "Zóna";
        public int CurrentPopulation { get { return _currentPopulation; } set { _currentPopulation = value; } }
        public Int32 Capacity { get { return _capacity; } set { _capacity = value; } }
        public ZoneType Type { get; set; }
        public Zone(int capacity, int currentPopulation)
        {
            Capacity = capacity;
            CurrentPopulation = currentPopulation;
            Name = "Zóna";
            Background = new SolidColorBrush(Colors.LightYellow);
        }
        public Zone(ZoneType type) : this(100, 0, type) { }
        public Zone(int capacity, int currentPopulation, ZoneType type)
        {
            Capacity = capacity;
            CurrentPopulation = currentPopulation;
            Type = type;
            Name = "Zóna";
            Background = new SolidColorBrush(Colors.LightYellow);
        }
    }

    public enum ZoneType
    {
        RESIDENTIAL,
        INDUSTRIAL,
        SERVICE,
        EMPTY,
    }
}
