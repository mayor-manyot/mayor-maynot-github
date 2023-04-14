using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Maynot.WPF.ViewModel
{
    public class Road : MaynotTile
    {
        private Int32 _buildCost;
        public Int32 BuildCost { get { return _buildCost; } set { _buildCost = value; } }
        public Road(int buildCost)
        {
            BuildCost = buildCost;
            Name = "Út";
        }
    }
    public class Empty : MaynotTile 
    {
        public Empty()
        { 
            Name = "Ü";
        }
    }
}
