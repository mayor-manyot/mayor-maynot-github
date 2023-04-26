using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaynotModel
{
    public class TimeElapsedEventArgs: EventArgs
    {
        public DateTime newDate;
        public TimeElapsedEventArgs(DateTime date)
        {
            this.newDate = date;
        }
    }
}
