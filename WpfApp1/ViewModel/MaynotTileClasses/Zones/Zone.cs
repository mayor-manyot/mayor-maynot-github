﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Maynot.WPF.ViewModel
{
    public class Zone : MaynotTile
    {
        private Int32 _capacity;
        private Int32 _currentPopulation;

        public override string DisplayName { get; } = "Zóna";
        public override SolidColorBrush Background { get; } = new SolidColorBrush(Colors.LightYellow);
        public int CurrentPopulation { get { return _currentPopulation; } set { _currentPopulation = value; } }
        public Int32 Capacity { get { return _capacity; } set { _capacity = value; } }
        public Zone(int capacity, int currentPopulation)
        {
            Capacity = capacity;
            CurrentPopulation = currentPopulation;
            Name = "Zóna";
        }
    }

}