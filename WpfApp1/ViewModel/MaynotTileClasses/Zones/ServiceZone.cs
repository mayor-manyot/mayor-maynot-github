using MaynotPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class ServiceZone : Zone
    {
        public override string DisplayName { get; } = "Szolgáltatás Zóna";
        public ServiceZone() : this(0, ZoneLevelsView.SMALL) { }
        public ServiceZone(int currentPopulation, ZoneLevelsView zoneLevel) : base(currentPopulation, zoneLevel) 
        {
            UpdateSprite();
        }

        public void UpdateSprite()
        {
            if (CurrentPopulation == 0)
            {
                SpriteImage = new BitmapImage(new Uri("/Assets/serviceZoneLevel0.png", UriKind.Relative));
            }
            else
            {
                switch (ZoneLevelView)
                {
                    case ZoneLevelsView.SMALL:
                        SpriteImage = new BitmapImage(new Uri("/Assets/serviceZoneLevel1.png", UriKind.Relative));
                        break;
                    case ZoneLevelsView.MEDIUM:
                        SpriteImage = new BitmapImage(new Uri("/Assets/serviceZoneLevel2.png", UriKind.Relative));
                        break;
                    case ZoneLevelsView.LARGE:
                        SpriteImage = new BitmapImage(new Uri("/Assets/serviceZoneLevel3.png", UriKind.Relative));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
