﻿using System;
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
    public class Bullet
    {
        public System.Windows.Shapes.Rectangle rectangle;
        public Rect hitbox;
        public double starterAngle;
        public double x;
        public double y;

        public Bullet(System.Windows.Shapes.Rectangle rectangle, Rect hitbox, double starterAngle, double x, double y)
        {
            this.rectangle = rectangle;
            this.hitbox = hitbox;
            this.starterAngle = starterAngle;
            this.x = x;
            this.y = y;
        }
    }
}
