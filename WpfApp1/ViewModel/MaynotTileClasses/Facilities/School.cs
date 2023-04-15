using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class School : Facility
    {
        public override string DisplayName { get; } = "Iskola";
        public School() : this(100, 0, 5000) { }
        public School(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost) 
        {
            Background = new SolidColorBrush(Colors.IndianRed);
        }
    }
}
