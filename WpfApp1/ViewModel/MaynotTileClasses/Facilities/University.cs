using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class University : Facility
    {
        public override string DisplayName { get; } = "Egyetem";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.Purple);
        public University() : this(100, 0, 5000) { }
        public University(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost)
        {

        }
    }
}
