using Maynot.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Maynot.WPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                // Zoom in
                Viewbox.Width = Math.Min(Viewbox.ActualWidth * 1.1, Viewbox.ActualWidth * 5);
                Viewbox.Height = Math.Min(Viewbox.ActualHeight * 1.1, Viewbox.ActualHeight * 5);
            }
            else
            {
                // Zoom out
                Viewbox.Width = Math.Max(Viewbox.ActualWidth / 1.1, Viewbox.ActualWidth / 5);
                Viewbox.Height = Math.Max(Viewbox.ActualHeight / 1.1, Viewbox.ActualHeight / 5);
            }

            e.Handled = true;
        }


    }

}
