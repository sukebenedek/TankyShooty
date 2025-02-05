using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankyShooty
{
    public class Bullet
    {
        public System.Windows.Shapes.Rectangle rectangle;
        public double starterAngle;
        public double x;
        public double y;

        public Bullet(System.Windows.Shapes.Rectangle rectangle, double starterAngle, double x, double y)
        {
            this.rectangle = rectangle;
            this.starterAngle = starterAngle;
            this.x = x;
            this.y = y;
        }
    }
}
