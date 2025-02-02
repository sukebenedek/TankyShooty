using System;
using System.Windows;
using System.Windows.Shapes; // Required for Rectangle in WPF
using System.Windows.Media;  // Required for Brushes
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace maze
{
    public class Wall
    {
        public static Random random = new Random();

        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }
        public SolidColorBrush color { get; set; }

        public Wall(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            byte g = (byte)random.Next(50, 150);
            color = new SolidColorBrush(Color.FromRgb(g, g, g));
        }

        public void Draw(Canvas cvs)
        {
            var g = (byte)random.Next(0, 256);
            Rectangle rect = new Rectangle
            {
                Width = (int)Math.Abs(x2 - x1),
                Height = (int)Math.Abs(y2 - y1),
                Fill = color,
            };

            Canvas.SetLeft(rect, x1);
            Canvas.SetTop(rect, y1);
            cvs.Children.Add(rect);
        }
    }
    public partial class MainWindow : Window
    {
        public static int cellWidth = 9;
        public Random random = new Random();
        public static int cellHeight = 6;
        public int unvisited = 0;
        public int cellSize;
        public List<Wall> walls = new List<Wall>();
        bool[,] visited = new bool[cellWidth, cellHeight];
        bool[,] vwalls = new bool[cellWidth, cellHeight];
        bool[,] hwalls = new bool[cellWidth, cellHeight];
        public int wallThickness = 12;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cellSize = (int)((cvs.ActualHeight) / cellHeight);
            cvs.Width = cellWidth * cellSize + 14; 
            this.Width = cvs.Width ;
            for (int i = 0; i < cellWidth; i++)
            {
                for (int j = 0; j < cellHeight; j++)
                {
                    vwalls[i, j] = true;
                    hwalls[i, j] = true;
                    visited[i, j] = false;
                }
            }
            AldousBroder();


            for (int i = 0; i < cellWidth; i++)
            {
                for (int j = 0; j < cellHeight; j++)
                {
                    if (vwalls[i, j] && i != 0)
                    {
                        walls.Add(new Wall((i) * cellSize, j * cellSize, (i) * cellSize + wallThickness, (j + 1) * cellSize + wallThickness));
                    }

                    if (hwalls[i, j] && j != 0)
                    {
                        walls.Add(new Wall(i * cellSize, (j) * cellSize, (i + 1) * cellSize, (j) * cellSize + wallThickness));
                    }
                }
            }

            walls.ForEach(w => w.Draw(cvs));
        }


        void AldousBroder()
        {
            int[] p = { 0, 0 };
            visit(p);

            while (unvisited < cellWidth * cellHeight)
            {
                var next = pickNeighbor(p);
                if (!isvisited(next))
                {
                    visit(next);
                    removeWall(p, next);
                }
                p = next;
            }
        }

        int[] pickNeighbor(int[] p)
        {
            int i = p[0], j = p[1];
            int[,] neighbors = new int[,] {
                {i-1, j}, {i+1, j}, {i, j+1}, {i, j-1}
            };
            return pickValid(neighbors);
        }

        void visit(int[] p)
        {
            visited[p[0], p[1]] = true;
            unvisited++;
        }

        int[] pick(int[,] neighbors)
        {
            int n = random.Next(4);
            return new int[] { neighbors[n, 0], neighbors[n, 1] };
        }

        bool checkValid(int[] point)
        {
            return (0 <= point[0] && point[0] < cellWidth) && (0 <= point[1] && point[1] < cellHeight);
        }

        int[] pickValid(int[,] neighbors)
        {
            var n = pick(neighbors);
            bool b = checkValid(n);
            while (!b)
            {
                n  = pick(neighbors);
                b = checkValid(n);
            }
            return n;
        }

        bool isvisited(int[] p)
        {
            return visited[p[0], p[1]];
        }

        void removeWall(int[] p1, int[] p2)
        {
            int dx = p2[0] - p1[0];
            int dy = p2[1] - p1[1];

            if (dx == -1) vwalls[p1[0], p1[1]] = false;
            if (dx == 1) vwalls[p1[0] + dx, p1[1]] = false;
            if (dy == -1) hwalls[p1[0], p1[1]] = false;
            if (dy == 1) hwalls[p1[0], p1[1] + dy] = false;
        }
    }
}