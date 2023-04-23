﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Maynot.WPF.ViewModel
{
    public class IndustrialZone : Zone
    {
        public override string DisplayName { get; } = "Ipari Zóna";
        public IndustrialZone() : this(100, 0) { }
        public IndustrialZone(int capacity, int currentPopulation) : base(capacity, currentPopulation) 
        {
            SpriteImage = new BitmapImage(new Uri("/Assets/industrialZone.png", UriKind.Relative));
        }
    }
}
