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
    public class Bulldozer : MaynotTile
    {
        public override string DisplayName { get; } = "Bulldozer";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.LightBlue);
        public Bulldozer()
        {
            Name = " ";
            SpriteImage = new BitmapImage(new Uri("/Assets/trashCan.png", UriKind.Relative));
        }
    }
}
