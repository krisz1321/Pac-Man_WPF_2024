using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pac_Man_WPF_2024
{
    public class Coin
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Ellipse Shape { get; set; }

        public Coin(int x, int y, double size, Brush color)
        {
            X = x;
            Y = y;
            Shape = new Ellipse
            {
                Width = size / 2,
                Height = size / 2,
                Fill = color
            };
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(Shape);
        }
    }


}
