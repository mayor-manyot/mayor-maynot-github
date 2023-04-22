using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaynotModel
{
    public class MaynotEventArg : EventArgs
    {
        private string info;
        private bool sucsess;
        private int x;
        private int y;

        public string Info { get => info; set => info = value; }
        public bool Sucsess { get => sucsess; set => sucsess = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public MaynotEventArg() { }
        public MaynotEventArg(string s)
        {
            info = s;
        }
        public MaynotEventArg(bool b) 
        {
            sucsess = b;
        }

        public MaynotEventArg(int x, int y ) 
        {
            this.y = y;
            this.x = x;
        }
    }
}
