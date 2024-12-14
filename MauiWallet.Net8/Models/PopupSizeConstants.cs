﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWallet.Models
{
    public class PopupSizeConstants
    {
        public PopupSizeConstants(IDeviceDisplay deviceDisplay)
        {
            Tiny = new(100, 100);
            Small = new(300, 300);
            Medium = new(0.7 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.6 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
            Large = new(0.9 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.8 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
        }


        public Size Tiny { get; }

        public Size Small { get; }


        public Size Medium { get; }

        public Size Large { get; }
    }
}
