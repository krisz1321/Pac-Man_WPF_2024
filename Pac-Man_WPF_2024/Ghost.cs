using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pac_Man_WPF_2024
{
    internal class Ghost
    {
        public int Id { get; set; }
        public Brush color {  get; set; }
        public int speed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }


    }
}
