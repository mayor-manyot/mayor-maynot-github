using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class PoliceStation : Facility
    {
        public override string DisplayName { get; } = "Rendőrség";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.Blue);
        public override int PriceToBuild { get; } = MaynotPersistence.PoliceStation.buildCost;
        public PoliceStation() : this(100, 0) { }
        public PoliceStation(int capacity, int currentPopulation) : base(capacity, currentPopulation, 100) 
        {
            SpriteImage = new BitmapImage(new Uri("/Assets/policeStation.png", UriKind.Relative));
        }
    }
}
