using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class Stadium : Facility
    {
        public override string DisplayName { get; } = "Stadion";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.Orange);
        public override int PriceToBuild { get; } = MaynotPersistence.Stadium.buildCost;
        public Stadium() : this(100, 0, 5000) { }
        public Stadium(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost)
        {
            SpriteImage = new BitmapImage(new Uri("/Assets/stadium.png", UriKind.Relative));
        }
    }
}
