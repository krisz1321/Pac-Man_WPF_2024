using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pac_Man_WPF_2024
{
    public class GameSettings
    {
        public Brush WallColor { get; set; } = Brushes.Yellow;       // A pálya falainak színe
        public Brush PathColor { get; set; } = Brushes.Black;      // Az útvonalak színe
        public int GhostCount { get; set; } = 4;                   // Szellemek száma a pályán
        public int Lives { get; set; } = 3;                        // Kezdő élet szám
        public int CellSize { get; set; } = 50;                    // Cella mérete (pixelekben)
        public bool Horror { get; set; } = false;
    }
}
