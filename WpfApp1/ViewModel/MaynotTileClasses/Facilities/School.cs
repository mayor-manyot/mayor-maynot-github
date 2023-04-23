using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class School : Facility
    {
        public override string DisplayName { get; } = "Iskola";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.IndianRed);
        public School() : this(100, 0, 5000) { }
        public School(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost) 
        {
            SpriteImage = new BitmapImage(new Uri("/Assets/school.png", UriKind.Relative));
        }
    }
}
