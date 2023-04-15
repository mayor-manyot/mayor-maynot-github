using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class Stadium : Facility
    {
        public override string DisplayName { get; } = "Stadion";
        public Stadium() : this(100, 0, 5000) { }
        public Stadium(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost)
        {
            Background = new SolidColorBrush(Colors.Orange);
        }
    }
}
