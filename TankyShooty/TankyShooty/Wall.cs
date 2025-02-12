using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Data;

namespace TankyShooty
{
    public class Wall
    {
        public static Random random = new Random();

        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }
        public SolidColorBrush color { get; set; }

        public Wall(int x1, int y1, int x2, int y2, byte? g = null)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            if (g == null) { g = (byte)random.Next(40, 180); }
            color = new SolidColorBrush(Color.FromRgb(g.Value, g.Value, g.Value));
        }

        public Rectangle Rect
        {
            get
            {
                var rect = new Rectangle
                {
                    Width = (int)Math.Abs(x2 - x1),
                    Height = (int)Math.Abs(y2 - y1),
                    Fill = color,
                    Tag = "Wall"
                };  

                Canvas.SetLeft(rect, x1);
                Canvas.SetTop(rect, y1);
                return rect;
            }
        }

        public void Draw(Canvas cvs)
        {
            cvs.Children.Add(Rect);
        }

        public void Remove(Canvas cvs)
        {
            cvs.Children.Remove(Rect);
        }
    }
}
