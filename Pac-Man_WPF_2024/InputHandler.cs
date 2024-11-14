﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// InputHandler.cs
using System.Windows.Input;

namespace Pac_Man_WPF_2024
{
    public class InputHandler
    {
        public int DeltaX { get; private set; }
        public int DeltaY { get; private set; }

        public void HandleKey(Key key)
        {
            DeltaX = 0;
            DeltaY = 0;

            switch (key)
            {
                case Key.W: // Fel
                case Key.Up:
                    DeltaY = -1;
                    break;
                case Key.S: // Le
                case Key.Down:
                    DeltaY = 1;
                    break;
                case Key.A: // Balra
                case Key.Left:
                    DeltaX = -1;
                    break;
                case Key.D: // Jobbra
                case Key.Right:
                    DeltaX = 1;
                    break;
            }
        }
    }
}
