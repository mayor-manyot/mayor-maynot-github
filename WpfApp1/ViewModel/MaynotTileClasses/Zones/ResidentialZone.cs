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
        public ResidentialZone() : this(100, 0) { }
        public ResidentialZone(int capacity, int currentPopulation) : base(capacity, currentPopulation) 
        {
            SpriteImage = new BitmapImage(new Uri("/Assets/residentalZoneLevel0.png", UriKind.Relative));
        }
    }
}
