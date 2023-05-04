using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maynot.WPF.ViewModel
{
    public class Person : ViewModelBase
    {
        public float _satisfaction;
        public float Satisfaction
        {
            get { return _satisfaction; }
            set
            {
                _satisfaction = value;
                OnPropertyChanged();
            }
        }
    }
}
