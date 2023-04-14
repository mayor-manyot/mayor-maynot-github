using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class MaynotTile : ViewModelBase
    {
        private String _name;
        public String Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public DelegateCommand? ClickCommand { get; set; }
    }
}
