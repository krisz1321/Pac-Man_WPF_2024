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
    public class Cherry
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Ellipse Shape { get; private set; }
        public bool Eaten { get; set; }

        public Cherry(int x, int y, double size)
        {
            X = x;
            Y = y;
            Eaten = false;
            Shape = new Ellipse
            {
                Width = size * 0.6,
                Height = size * 0.6,
                Fill = Brushes.Red,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 2
            };
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(Shape);
        }
    }

}
