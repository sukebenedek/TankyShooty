using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveForwardPlayer1, moveBackwardPlayer1, moveForwardPlayer2, moveBackwardPlayer2 = false;
        bool rotateLeftPlayer1, rotateRightPlayer1, rotateLeftPlayer2, rotateRightPlayer2 = false;
        int playerSpeed = 6;
        int rotateSpeed = 5;
        int bulletSpeed = 15;
        List<System.Windows.Shapes.Rectangle> itemRemover = new List<System.Windows.Shapes.Rectangle>();
        List<int> scores = new List<int>(2);
        List<Bullet> bullets = new List<Bullet>();

        Rect player1Hitbox;
        Rect player2Hitbox;
        public GameWindow()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            MyCanvas.Focus();

            //ImageBrush player1Image = new ImageBrush();
            //player1Image.ImageSource = new BitmapImage(new Uri("url here"));
            //Player1.Fill = playerImgae();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            player1Hitbox = new Rect(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), Player1.Width, Player1.Height);
            player2Hitbox = new Rect(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), Player1.Width, Player1.Height);
            //rotation.Content = rectangleRotatePlayer1.Angle + " - " + Math.Sin(rectangleRotatePlayer1.Angle) + " - " + Math.Cos(rectangleRotatePlayer1.Angle);
            //scoreText.Content = $"{scores[0]} - {scores[1]}";
            rotation.Content = bullets.Count;

            if (moveForwardPlayer1 == true)
            {

                MovePlayer(Player1, rectangleRotatePlayer1, true);

            }
            if(moveBackwardPlayer1 == true)
            {
                MovePlayer(Player1, rectangleRotatePlayer1, false);
            }
            if (rotateLeftPlayer1 == true) 
            {
                RotatePlayer(rectangleRotatePlayer1, false, rotateSpeed);
            }
            if (rotateRightPlayer1 == true)
            {
                RotatePlayer(rectangleRotatePlayer1, true, rotateSpeed);
            }


            if (moveForwardPlayer2 == true)
            {
                MovePlayer(Player2, rectangleRotatePlayer2, true);
            }
            if (moveBackwardPlayer2 == true)
            {
                MovePlayer(Player2, rectangleRotatePlayer2, false);
            }

            if (rotateLeftPlayer2 == true)
            {
                RotatePlayer(rectangleRotatePlayer2, false, rotateSpeed);
            }
            if (rotateRightPlayer2 == true)
            {
                RotatePlayer(rectangleRotatePlayer2, true, rotateSpeed);
            }

            //foreach (var x in MyCanvas.Children.OfType<System.Windows.Shapes.Rectangle>())
            //{
            //    if (x is System.Windows.Shapes.Rectangle && (string)x.Tag == "bulletPlayer1")
            //    {
            //        double angle = rectangleRotatePlayer1.Angle;

            //        double angleInRadians = angle * (Math.PI / 180);


            //        double deltaX = Math.Cos(angleInRadians) * playerSpeed;
            //        double deltaY = Math.Sin(angleInRadians) * playerSpeed;


            //        double currentLeft = Canvas.GetLeft(x);
            //        double currentTop = Canvas.GetTop(x);

            //        Canvas.SetLeft(x, currentLeft + deltaX);
            //        Canvas.SetTop(x, currentTop + deltaY);

            //    }
            //}

            foreach (var bullet in bullets)
            {

                
                double angle = bullet.starterAngle;

                double angleInRadians = angle * (Math.PI / 180);


                double deltaX = Math.Cos(angleInRadians) * bulletSpeed;
                double deltaY = Math.Sin(angleInRadians) * bulletSpeed;


                double currentLeft = Canvas.GetLeft(bullet.rectangle);
                double currentTop = Canvas.GetTop(bullet.rectangle);

                Canvas.SetLeft(bullet.rectangle, currentLeft + deltaX);
                Canvas.SetTop(bullet.rectangle, currentTop + deltaY);

                
            }
        }

        private void MovePlayer(System.Windows.Shapes.Rectangle player,RotateTransform rotateTransform, bool forward)
        {
            double angle = rotateTransform.Angle;

            double angleInRadians = angle * (Math.PI / 180);


            double deltaX = Math.Cos(angleInRadians) * playerSpeed;
            double deltaY = Math.Sin(angleInRadians) * playerSpeed;


            double currentLeft = Canvas.GetLeft(player);
            double currentTop = Canvas.GetTop(player);

            if (forward) 
            {
                Canvas.SetLeft(player, currentLeft + deltaX);
                Canvas.SetTop(player, currentTop + deltaY);
            }
            else
            {
                Canvas.SetLeft(player, currentLeft - deltaX);
                Canvas.SetTop(player, currentTop - deltaY);
            }

            
            
        }

        private void RotatePlayer(RotateTransform rotateTransform, bool direction_positive, int rotateSpeed)
        {
            if (direction_positive) rotateTransform.Angle += rotateSpeed;
            else rotateTransform.Angle -= rotateSpeed;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) 
            {
                moveForwardPlayer1 = true;
            }
            if (e.Key == Key.Down) 
            {
                moveBackwardPlayer1 = true;
            }
            if(e.Key == Key.Left)
            {
                rotateLeftPlayer1 = true;
            }
            if (e.Key == Key.Right)
            {
                rotateRightPlayer1 = true;
            }

            if (e.Key == Key.W)
            {
                moveForwardPlayer2 = true;
            }
            if (e.Key == Key.S)
            {
                moveBackwardPlayer2 = true;
            }

            if (e.Key == Key.A)
            {
                rotateLeftPlayer2 = true;
            }
            if (e.Key == Key.D)
            {
                rotateRightPlayer2 = true;
            }


        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                moveForwardPlayer1 = false;
            }
            if (e.Key == Key.Down)
            {
                moveBackwardPlayer1 = false;
            }
            if (e.Key == Key.Left)
            {
                rotateLeftPlayer1 = false;
            }
            if (e.Key == Key.Right)
            {
                rotateRightPlayer1 = false;
            }

            if (e.Key == Key.W)
            {
                moveForwardPlayer2 = false;
            }
            if (e.Key == Key.S)
            {
                moveBackwardPlayer2 = false;
            }

            if (e.Key == Key.A)
            {
                rotateLeftPlayer2 = false;
            }
            if (e.Key == Key.D)
            {
                rotateRightPlayer2 = false;
            }

            if (e.Key == Key.Space)
            {
                System.Windows.Shapes.Rectangle newBulletPlayer1 = new System.Windows.Shapes.Rectangle
                {
                    Tag = "bulletPlayer1",
                    Height = 5,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red,
                };

                Canvas.SetLeft(newBulletPlayer1, Canvas.GetLeft(Player1) + Player2.Width / 2);
                Canvas.SetTop(newBulletPlayer1, Canvas.GetTop(Player1));

                MyCanvas.Children.Add(newBulletPlayer1);

                bullets.Add(new Bullet(newBulletPlayer1, rectangleRotatePlayer1.Angle, Canvas.GetLeft(Player1) + Player2.Width / 2, 0));
            }

            if (e.Key == Key.R)
            {
                System.Windows.Shapes.Rectangle newBulletPlayer2 = new System.Windows.Shapes.Rectangle
                {
                    Tag = "bulletPlayer2",
                    Height = 5,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red,
                };

                Canvas.SetLeft(newBulletPlayer2, Canvas.GetLeft(Player2) + Player2.Width / 2);
                Canvas.SetTop(newBulletPlayer2, Canvas.GetTop(Player2));

                MyCanvas.Children.Add(newBulletPlayer2);

                bullets.Add(new Bullet(newBulletPlayer2, rectangleRotatePlayer2.Angle, Canvas.GetLeft(Player2) + Player2.Width / 2, 0));
            }
        }
    }
}
