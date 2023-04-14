using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class Forest : MaynotTile
    {
        private Int32 _age;
        private Int32 _buildCost;
        public Int32 Age { get { return _age; } set { _age = value; } }
        public Int32 BuildCost { get { return _buildCost; } set { _buildCost = value; } }
        public Forest(int age, int buildCost)
        {
            Age = age;
            BuildCost = buildCost;
        }
    }
}
