using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Pac_Man_WPF_2024
{
    public class PacMan
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; } = 50;
        public Brush Color { get; set; } = Brushes.Yellow;

        public Ellipse Shape { get; private set; }

        public PacMan(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Shape = new Ellipse
            {
                Width = Size,
                Height = Size,
                Fill = Color
            };
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            Canvas.SetLeft(Shape, X * Size);
            Canvas.SetTop(Shape, Y * Size);
        }

        public void Move(int deltaX, int deltaY, int[,] map)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            // Ellenőrzés ütközik-e fallal
            // (1-es érték a fal)
            if (newX >= 0 && newX < map.GetLength(1) && newY >= 0 && newY < map.GetLength(0) && map[newY, newX] == 0)
            {
                X = newX;
                Y = newY;
            }
            UpdatePosition();
        }

    }
}
