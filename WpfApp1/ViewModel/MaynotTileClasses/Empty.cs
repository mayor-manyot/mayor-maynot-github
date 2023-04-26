using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class Empty : MaynotTile
    {
        public static BitmapImage EmptyDefaultSprite = new BitmapImage(new Uri("/Assets/RoadTiles/roadPLAZA.jpg", UriKind.Relative));
        public override string DisplayName { get; } = "Üres";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.LightBlue);
        public Empty()
        {
            Name = " ";
            SpriteImage = new BitmapImage(new Uri("/Assets/RoadTiles/roadPLAZA.jpg", UriKind.Relative));
        }
    }
}
