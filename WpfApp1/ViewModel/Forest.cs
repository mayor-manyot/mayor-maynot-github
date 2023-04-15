using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class Forest : MaynotTile
    {
        private Int32 _age;
        private Int32 _buildCost;
        public Int32 Age { get { return _age; } set { _age = value; } }
        public Int32 BuildCost { get { return _buildCost; } set { _buildCost = value; } }
        public override string DisplayName { get; } = "Erdő";
        public Forest() : this(0, 5000) { }
        public Forest(int age, int buildCost)
        {
            Age = age;
            BuildCost = buildCost;
            Background = new System.Windows.Media.SolidColorBrush(Colors.ForestGreen);
        }
    }
}
