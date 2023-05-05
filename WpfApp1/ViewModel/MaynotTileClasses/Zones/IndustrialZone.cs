using MaynotPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class IndustrialZone : Zone
    {
        public override string DisplayName { get; } = "Ipari Zóna";
        public IndustrialZone() : this(0, ZoneLevelsView.SMALL) { }
        public IndustrialZone(int currentPopulation, ZoneLevelsView zoneLevel) : base(currentPopulation, zoneLevel) 
        {
            UpdateSprite();
        }

        public void UpdateSprite()
        {
            if (CurrentPopulation == 0)
            {
                SpriteImage = new BitmapImage(new Uri("/Assets/industrialZoneLevel0.png", UriKind.Relative));
            }
            else
            {
                switch (ZoneLevelView)
                {
                    case ZoneLevelsView.SMALL:
                        SpriteImage = new BitmapImage(new Uri("/Assets/industrialZoneLevel1.png", UriKind.Relative));
                        break;
                    case ZoneLevelsView.MEDIUM:
                        SpriteImage = new BitmapImage(new Uri("/Assets/industrialZoneLevel2.png", UriKind.Relative));
                        break;
                    case ZoneLevelsView.LARGE:
                        SpriteImage = new BitmapImage(new Uri("/Assets/industrialZoneLevel3.png", UriKind.Relative));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
