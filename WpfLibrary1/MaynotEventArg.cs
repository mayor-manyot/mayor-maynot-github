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

        public string Info { get => info; set => info = value; }
        public bool Sucsess { get => sucsess; set => sucsess = value; }

        public MaynotEventArg() { }
        public MaynotEventArg(string s)
        {
            info = s;
        }
        public MaynotEventArg(bool b) 
        {
            sucsess = b;
        }
    }
}
