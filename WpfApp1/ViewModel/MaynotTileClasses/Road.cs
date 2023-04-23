using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Windows.Resources;
using System.Windows;

namespace Maynot.WPF.ViewModel
{
    public class Road : MaynotTile
    {
        private Int32 _buildCost;
        public Int32 BuildCost { get { return _buildCost; } set { _buildCost = value; } }

        public override string DisplayName { get; } = "Út";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.SlateGray);
        public Road() : this(5000) { }
        public Road(int buildCost)
        {
            BuildCost = buildCost;
            Name = "Út";
            SpriteImage = new BitmapImage(new Uri("/Assets/RoadTiles/roadEW.jpg", UriKind.Relative));

        }

        public void SetRoadSprite(bool north, bool east, bool south, bool west)
        {
            string spriteName = "road";
            int neighborCount = 0;

            if (north) { spriteName += "N"; neighborCount++; }
            if (east) { spriteName += "E"; neighborCount++; }
            if (west) { spriteName += "W"; neighborCount++; }
            if (south) { spriteName += "S"; neighborCount++; }

            if (neighborCount == 0)
            {
                spriteName = "roadEW";
            }
            else if (neighborCount == 1)
            {
                if (north || south) spriteName = "roadNS";
                else if (east || west) spriteName = "roadEW";
            }

            //string imagePath = Path.Combine("Assets", "RoadTiles", spriteName);
            string imagePath = "/Assets/RoadTiles/" + spriteName + ".jpg";
            //Debug.WriteLine("north, east, west, south " + north + east + west + south);
            
            //Debug.WriteLine("Path amit ad: " + imagePath);
            SpriteImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            //Debug.WriteLine("Megegyezenek: " + ("/Assets/RoadTiles/roadEW.jpg" == imagePath));
            //StreamResourceInfo info = Application.GetResourceStream(new Uri(imagePath, UriKind.Relative));
            //Debug.WriteLine("Létezik a path: " + info != null);
        }

    }

}
