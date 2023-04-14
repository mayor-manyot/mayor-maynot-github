using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Maynot.WPF.ViewModel
{
    public class MaynotTile : ViewModelBase
    {
        private bool _canBeDemolished;
        public bool CanBeDemolished { get; private set; }
        public Image SpriteImage { get; set; }
        public int MyProperty { get; set; }
       
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public DelegateCommand? ClickCommand { get; set; }
    }
}
