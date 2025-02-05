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

        int startXPlayer1 = 410;
        int startYPlayer1 = 410;
        int startXPlayer2 = 40;
        int startYPlayer2 = 40;

        string player1Name = "Player1";
        string player2Name = "Player2";

        List<System.Windows.Shapes.Rectangle> itemRemover = new List<System.Windows.Shapes.Rectangle>();
        List<int> scores = [0, 0];
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

            ImageBrush imageBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetFiles(Directory.GetCurrentDirectory() + "/img/", "*.jpg").ToList()[0]))
            };
            Player1.Fill = imageBrush;

            ImageBrush imageBrush2 = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetFiles(Directory.GetCurrentDirectory() + "/img/", "*.jpg").ToList()[2]))
            };
            Player2.Fill = imageBrush2;

            player1NameDisplay.Content = player1Name;
            player2NameDisplay.Content = player2Name;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            player1Hitbox = new Rect(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), Player1.Width, Player1.Height);
            player2Hitbox = new Rect(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), Player1.Width, Player1.Height);

            rotation.Content = rectangleRotatePlayer1.Angle + " - " + Math.Sin(rectangleRotatePlayer1.Angle) + " - " + Math.Cos(rectangleRotatePlayer1.Angle);
            scoreText.Content = $"{scores[0]} - {scores[1]}";
            //rotation.Content = bullets.Count;

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
                bullet.hitbox = new Rect(Canvas.GetLeft(bullet.rectangle), Canvas.GetTop(bullet.rectangle), 5, 5);

                if ((string)bullet.rectangle.Tag == "bulletPlayer1")
                {
                    if (player2Hitbox.IntersectsWith(bullet.hitbox))
                    {
                        RemoveBullets();
                        ResetPlayers(-90.0);
                        scores[1]++;
                    }  
                }
                if ((string)bullet.rectangle.Tag == "bulletPlayer2")
                {
                    if (player1Hitbox.IntersectsWith(bullet.hitbox))
                    {
                        RemoveBullets();
                        ResetPlayers(90.0);
                        scores[0]++;
                    }

                }

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

        private void ResetPlayers(double angle) 
        {
            Canvas.SetLeft(Player1, startXPlayer1);
            Canvas.SetTop(Player1, startYPlayer1);
            rectangleRotatePlayer1.Angle = angle;
            Canvas.SetLeft(Player2, startXPlayer2);
            Canvas.SetTop(Player2, startYPlayer2);
            rectangleRotatePlayer2.Angle = angle;
        }

        private void RemoveBullets()
        {
            foreach (var bullet in bullets)
            {
                MyCanvas.Children.Remove(bullet.rectangle);
                
            }
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

                Canvas.SetLeft(newBulletPlayer1, GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 30.0, 30.0, rectangleRotatePlayer1.Angle)[0] - (5 / 2));
                Canvas.SetTop(newBulletPlayer1, GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 30.0, 30.0, rectangleRotatePlayer1.Angle)[1]);

                Rect bulletHitBox = new Rect(Canvas.GetLeft(Player1) + Player1.Width / 2, GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 30.0, 30.0, rectangleRotatePlayer1.Angle)[1], 5, 5);

                MyCanvas.Children.Add(newBulletPlayer1);

                bullets.Add(new Bullet(newBulletPlayer1, bulletHitBox, rectangleRotatePlayer1.Angle, GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 30.0, 30.0, rectangleRotatePlayer1.Angle)[0], GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 30.0, 30.0, rectangleRotatePlayer1.Angle)[1]));
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



                Canvas.SetLeft(newBulletPlayer2, GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[0] - (5 / 2));
                Canvas.SetTop(newBulletPlayer2, GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[1]);

                Rect bulletHitBox = new Rect(GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[0] - (5 / 2), GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[1], 5, 5);

                MyCanvas.Children.Add(newBulletPlayer2);

                bullets.Add(new Bullet(newBulletPlayer2, bulletHitBox, rectangleRotatePlayer2.Angle, GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[0], GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[1]));
            }
        }

        public double[] GetMiddleOfFrontEdge(double x, double y, double width = 30.0, double height = 30.0, double angle = 0)
        {
            // Convert angle to radians
            double radians = angle * Math.PI / 180;

            // Center of the rectangle
            double centerX = x + width / 2;
            double centerY = y + height / 2;

            // Half the width of the rectangle (front edge direction)
            double halfWidth = width / 2;

            // The position of the front edge midpoint after rotation
            double frontEdgeX = centerX + halfWidth * Math.Cos(radians);
            double frontEdgeY = centerY + halfWidth * Math.Sin(radians);

            return [frontEdgeX, frontEdgeY];
        }
    }
}
