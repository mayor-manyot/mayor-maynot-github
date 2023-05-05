using MaynotPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class ResidentialZone : Zone
    {
        public override string DisplayName { get; } = "Lakó Zóna";
        public ResidentialZone() : this(0, ZoneLevelsView.SMALL) { }
        public ResidentialZone(int currentPopulation, ZoneLevelsView zoneLevel) : base(currentPopulation, zoneLevel) 
        {
            UpdateSprite();
        }

        public void UpdateSprite()
        {
            if (CurrentPopulation == 0)
            {
                SpriteImage = new BitmapImage(new Uri("/Assets/residentalZoneLevel0.png", UriKind.Relative));
            }
            else
            {
                switch (ZoneLevelView)
                {
                    case ZoneLevelsView.SMALL:
                        if (CurrentPopulation <= 15)
                        {
                            SpriteImage = new BitmapImage(new Uri("/Assets/residentalZoneLevel1Small.png", UriKind.Relative));
                        }
                        else
                        {
                            SpriteImage = new BitmapImage(new Uri("/Assets/residentalZoneLevel1Big.png", UriKind.Relative));
                        }
                        break;
                    case ZoneLevelsView.MEDIUM:
                        SpriteImage = new BitmapImage(new Uri("/Assets/residentalZoneLevel2.png", UriKind.Relative));
                        break;
                    case ZoneLevelsView.LARGE:
                        SpriteImage = new BitmapImage(new Uri("/Assets/residentalZoneLevel3.png", UriKind.Relative));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
