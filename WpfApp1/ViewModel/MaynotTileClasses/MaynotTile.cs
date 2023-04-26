using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class MaynotTile : ViewModelBase
    {
        private bool _canBeDemolished;
        private string _name;
        private Visibility isFlameVisible = Visibility.Hidden;
        public string? SpriteImagePath { get; set; }

        private SolidColorBrush _background;
        private BitmapImage _spriteImage;
        public bool CanBeDemolished { get; private set; }
        public Visibility IsFlameVisible {
            get { return isFlameVisible; }
            set
            {
                isFlameVisible = value;
                OnPropertyChanged(nameof(IsFlameVisible));
            }
        }
        public BitmapImage? SpriteImage {
            get { return _spriteImage; }
            set
            {
                _spriteImage = value;
                OnPropertyChanged(nameof(SpriteImage));
            }
        }
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

        public virtual SolidColorBrush Background {
            get
            {
                return _background;
            }
        }
    }
}
