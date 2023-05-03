using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaynotPersistence
{
    public class Forest: Tile
    {
        /// <summary>
        /// az erdők építési költsége
        /// </summary>
        public static Int32 buildCost;
        public DateTime PlantingDate { get; }
        public Int32 Age { get; set; }
        public Forest(DateTime plantingDate)
        {
            Age = 0;
            PlantingDate = plantingDate;
        }
    }
}
