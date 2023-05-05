using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class University : Facility
    {
        public override string DisplayName { get; } = "Egyetem";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.Purple);
        public override int PriceToBuild { get; } = MaynotPersistence.University.buildCost;
        public University() : this(100, 0, 5000) { }
        public University(int capacity, int currentPopulation, int buildCost) : base(capacity, currentPopulation, buildCost)
        {
            SpriteImage = new BitmapImage(new Uri("/Assets/university.png", UriKind.Relative));
        }
    }
}
