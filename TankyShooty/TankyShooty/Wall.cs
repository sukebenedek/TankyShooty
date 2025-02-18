using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            if (g == null) { g = (byte)random.Next(40, 120); }
            color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(g.Value, g.Value, g.Value));
        }

        public System.Windows.Shapes.Rectangle Rect
        {
            get
            {
                var rect = new System.Windows.Shapes.Rectangle
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

        public bool isVertical
        {
            get
            {
                return (Hitbox.Width > Hitbox.Height) ? false : true; // ts pmo 💔
            }
        }

        public Rect Hitbox
        {
            get
            {
                int left = x1;
                int top = y1;
                int width = (int)Math.Abs(x2 - x1);
                int height = (int)Math.Abs(y2 - y1);

                return new Rect(left, top, width, height);
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
