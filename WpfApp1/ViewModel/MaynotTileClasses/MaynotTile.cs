using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class MaynotTile : ViewModelBase
    {
        private bool _canBeDemolished;
        private string _name;
        private SolidColorBrush _background;
        public bool CanBeDemolished { get; private set; }
        public Image? SpriteImage { get; set; }
        public virtual string DisplayName { get; } // statikusan felülírja az összes gyerekosztáj
        public string Name {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
       
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public DelegateCommand? ClickCommand { get; set; }

        public SolidColorBrush Background {
            get
            {
                return _background;
            }
            set {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }
    }
}
