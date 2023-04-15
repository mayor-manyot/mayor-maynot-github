using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class Road : MaynotTile
    {
        private Int32 _buildCost;
        public Int32 BuildCost { get { return _buildCost; } set { _buildCost = value; } }
        public Road() : this(5000) { }
        public Road(int buildCost)
        {
            BuildCost = buildCost;
            Name = "Út";
            Background = new SolidColorBrush(Colors.SlateGray);
        }
    }
    
}
