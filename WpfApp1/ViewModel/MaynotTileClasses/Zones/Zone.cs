using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static Maynot.WPF.ViewModel.Zone;

namespace Maynot.WPF.ViewModel
{
    public class Zone : MaynotTile
    {
        private Int32 _capacity;
        private Int32 _currentPopulation;
        public enum ZoneLevelsView
        {
            SMALL = 1,
            MEDIUM = 2,
            LARGE = 3,
        }
        public ZoneLevelsView ZoneLevelView { get; set; }
        public override string DisplayName { get; } = "Zóna";
        public override int PriceToBuild { get; } = 0;
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.LightYellow);
        public int CurrentPopulation { get { return _currentPopulation; } set { _currentPopulation = value; } }
        public Int32 Capacity { get { return _capacity; } set { _capacity = value; } }
        public Zone(int currentPopulation, ZoneLevelsView zoneLevelView)
        {
            ZoneLevelView = zoneLevelView;
            CurrentPopulation = currentPopulation;
            Name = "Zóna";
        }
    }

}
