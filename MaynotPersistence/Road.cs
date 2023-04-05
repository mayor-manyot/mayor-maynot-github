using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaynotPersistence
{
    public class Road: Tile
    {
        /// <summary>
        /// utak építési költsége
        /// </summary>
        public static Int32 buildCost;

        /// <summary>
        /// utak fentarási költsége
        /// </summary>
        public static Int32 maintenanceFee;
        public Road()
        {}
    }
}
