using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class Facility : MaynotTile
    {
        private int _capacity;
        private int _currentPopulation;
        public int CurrentPopulation { get { return _currentPopulation; } set { _currentPopulation = value; } }
        public int Capacity { get { return _capacity; } set { _capacity = value; } }
        public override string DisplayName { get; } = "Facility";
        
        public Facility(int capacity, int currentPopulation, int buildCost)
        {
            Capacity = capacity;
            CurrentPopulation = currentPopulation;
        }
    }

}
