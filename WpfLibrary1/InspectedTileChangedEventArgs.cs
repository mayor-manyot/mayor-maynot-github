using MaynotPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaynotModel
{
    public class InspectedTileChangedEventArgs: EventArgs
    {
        public Tile _tile;
        public InspectedTileChangedEventArgs(Tile tile)
        {
            this._tile = tile;
        }
    }
}
