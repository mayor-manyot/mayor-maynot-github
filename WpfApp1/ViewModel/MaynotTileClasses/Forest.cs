using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class Forest : MaynotTile
    {
        private Int32 _age;
        private Int32 _buildCost;
        public Int32 Age { get { return _age; } set { _age = value; } }
        public Int32 BuildCost { get { return _buildCost; } set { _buildCost = value; } }
        public override string DisplayName { get; } = "Erdő";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.ForestGreen);
        public override int PriceToBuild { get; } = MaynotPersistence.Forest.BuildCost;
        public Forest() : this(0, 5000) { }
        public Forest(int age, int buildCost)
        {
            Age = age;
            BuildCost = buildCost;
            SpriteImage = new BitmapImage(new Uri("/Assets/forest.png", UriKind.Relative));
        }
    }
}
