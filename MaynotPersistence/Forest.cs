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
        public static Int32 BuildCost { get; set; } = 6759;
        public static Int32 MaintenanceFee { get; set; } = 3759;
        public Int32 Age { get; set; } = 0;
        public DateTime PlantingDate { get; }       
        public bool Generated { get; set; }
        public Forest(DateTime plantingDate, bool generated)
        {           
            PlantingDate = plantingDate;
            Generated = generated;
        }
    }
}
