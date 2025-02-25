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
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    /// 
    public partial class GameWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveForwardPlayer1, moveBackwardPlayer1, moveForwardPlayer2, moveBackwardPlayer2 = false;
        bool rotateLeftPlayer1, rotateRightPlayer1, rotateLeftPlayer2, rotateRightPlayer2 = false;
        int playerSpeed = 6;
        int rotateSpeed = 5;
        int bulletSpeed = 15;

        int startXPlayer1 = 900;
        int startYPlayer1 = 900;
        int startXPlayer2 = 25;
        int startYPlayer2 = 10;

        string player1Name = "Player1";
        string player2Name = "Player2";

        public static Random random = new Random();

        
        List<Bullet> bullets = new List<Bullet>();
        List<Bullet> bulletsToRemove = new List<Bullet>();

        Rect player1Hitbox;
        Rect player2Hitbox;

        public static int cellWidth = 6;
        public static int cellHeight = 6;
        public int wallThickness = 15;
        public static int height;
        public static int width;
        public int cellSize;

        public List<Wall> walls = new List<Wall>();
        public int unvisited = 0;
        bool[,] visited = new bool[cellWidth, cellHeight];
        bool[,] vwalls = new bool[cellWidth, cellHeight];
        bool[,] hwalls = new bool[cellWidth, cellHeight];



        public GameWindow()
        {
            InitializeComponent();

            cellSize = 150;


            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop!;
            gameTimer.Start();

            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/Canvas-Background.jpg"));
            bg.TileMode = TileMode.None;
            bg.Stretch = Stretch.Fill;
            MyCanvas.Background = bg;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            height = Convert.ToInt32(MyCanvas.ActualHeight);
            width = Convert.ToInt32(MyCanvas.ActualWidth);

            for (int i = 0; i < cellWidth; i++)
            {
                for (int j = 0; j < cellHeight; j++)
                {
                    vwalls[i, j] = true;
                    hwalls[i, j] = true;
                    visited[i, j] = false;
                }
            }

            GenerateMaze();

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

            walls.Add(new Wall(0, 0 , wallThickness, height));
            walls.Add(new Wall(0, 0 , width, wallThickness));
            walls.Add(new Wall(width - wallThickness, 0 , width, height));
            walls.Add(new Wall(0, height - wallThickness , width, height));

            walls.ForEach(w => w.Draw(MyCanvas));

        }

        private void GameLoop(object sender, EventArgs e)
        {
            //rotation.Content = GameData.Skin1;
            player1Hitbox = new Rect(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), Player1.Width, Player1.Height);
            player2Hitbox = new Rect(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), Player2.Width, Player2.Height);

            List<string> lines = [];
            if (GameData.Skin1 != 0)
            {
                try
                {
                    ImageBrush imageBrush = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/" + (GameData.Skin1) + ".jpg"))
                    };
                    Player1.Fill = imageBrush;

                    ImageBrush imageBrush2 = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/" + (GameData.Skin2) + ".jpg"))
                    };
                    Player2.Fill = imageBrush2;

                    player1NameDisplay.Content = GameData.Name1;
                    player2NameDisplay.Content = GameData.Name2;
                }
                catch (Exception ex) 
                {
                    ImageBrush imageBrush = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/1.jpg"))
                    };
                    Player1.Fill = imageBrush;

                    ImageBrush imageBrush2 = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/2.jpg"))
                    };
                    Player2.Fill = imageBrush2;

                    player1NameDisplay.Content = player1Name;
                    player2NameDisplay.Content = player2Name;
                }
            }
            else
            {
                
                try
                {
                    lines = File.ReadAllLines("nevek.txt").ToList();
                }
                catch (Exception ex) 
                {
                    ImageBrush imageBrush = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/1.jpg"))
                    };
                    Player1.Fill = imageBrush;

                    ImageBrush imageBrush2 = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/2.jpg"))
                    };
                    Player2.Fill = imageBrush2;

                    player1NameDisplay.Content = player1Name;
                    player2NameDisplay.Content = player2Name;
                }
                
                if (lines.Count > 0)
                {
                    try
                    {
                        ImageBrush imageBrush = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/" + lines[0].Split(';')[1]))
                        };
                        Player1.Fill = imageBrush;

                        ImageBrush imageBrush2 = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/" + lines[1].Split(';')[1]))
                        };
                        Player2.Fill = imageBrush2;

                        player1NameDisplay.Content = lines[0].Split(';')[0];
                        player2NameDisplay.Content = lines[1].Split(';')[0];
                    }
                    catch (Exception ex) 
                    {
                        ImageBrush imageBrush = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/1.jpg"))
                        };
                        Player1.Fill = imageBrush;

                        ImageBrush imageBrush2 = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/2.jpg"))
                        };
                        Player2.Fill = imageBrush2;

                        player1NameDisplay.Content = player1Name;
                        player2NameDisplay.Content = player2Name;
                    }
                    
                }
                else
                {
                    ImageBrush imageBrush = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/1.jpg"))
                    };
                    Player1.Fill = imageBrush;

                    ImageBrush imageBrush2 = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/players/2.jpg"))
                    };
                    Player2.Fill = imageBrush2;

                    player1NameDisplay.Content = player1Name;
                    player2NameDisplay.Content = player2Name;
                }
            }

            scoreText.Content = $"{Score.Scores[0]} - {Score.Scores[1]}";

            if (moveForwardPlayer1 == true)
            {
                if (CanMoveForward(Player1, rectangleRotatePlayer1, player1Hitbox))
                {
                    MovePlayer(Player1, rectangleRotatePlayer1, true);
                }
  
            }

            if (moveBackwardPlayer1 == true)
            {
                if (CanMoveBackward(Player1, rectangleRotatePlayer1, player1Hitbox))
                {
                    MovePlayer(Player1, rectangleRotatePlayer1, false);
                }  
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
                if (CanMoveForward(Player2, rectangleRotatePlayer2, player2Hitbox))
                {
                    MovePlayer(Player2, rectangleRotatePlayer2, true);
                }
            }

            if (moveBackwardPlayer2 == true)
            {
                if (CanMoveBackward(Player2, rectangleRotatePlayer2, player2Hitbox))
                {
                    MovePlayer(Player2, rectangleRotatePlayer2, false);
                }
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
                        ResetPlayers(-90.0, 90.0);
                        Score.Scores[1]++;
                        Die();

                    }
                }
                if ((string)bullet.rectangle.Tag == "bulletPlayer2")
                {
                    if (player1Hitbox.IntersectsWith(bullet.hitbox))
                    {
                        RemoveBullets();
                        ResetPlayers(-90.0, 90.0);
                        Score.Scores[0]++;
                        Die();

                    }

                }

                foreach (var wall in walls)
                {
                    if(bullet.hitbox.IntersectsWith(wall.Hitbox))
                    {
                        MyCanvas.Children.Remove(bullet.rectangle);
                        bulletsToRemove.Add(bullet);
                    }
                }

            }
            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
            }

        }

        private Rect RotateRect(Rect rect, double angle)
        {
            // Convert the angle to radians
            double angleInRadians = angle * (Math.PI / 180);

            // Get the center of the rectangle
            double centerX = rect.X + rect.Width / 2;
            double centerY = rect.Y + rect.Height / 2;

            // Get the new top-left coordinates after rotation
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            double offsetX = rect.X - centerX;
            double offsetY = rect.Y - centerY;

            // Apply rotation formula for both X and Y axes
            double newX = centerX + offsetX * cosTheta - offsetY * sinTheta;
            double newY = centerY + offsetX * sinTheta + offsetY * cosTheta;

            // Return the new rotated Rect
            return new Rect(newX, newY, rect.Width, rect.Height);
        }

        private bool CanMoveForward(System.Windows.Shapes.Rectangle player, RotateTransform rotateTransform, Rect hitbox)
        {
            foreach (var wall in walls)
            {
                
                if (hitbox.IntersectsWith(wall.Hitbox))
                {
                    if(wall.isVertical) //VERTIVAL WALL
                    {
                        if (Canvas.GetLeft(wall.Rect) >= Canvas.GetLeft(player) && (Canvas.GetTop(wall.Rect) < Canvas.GetTop(player)) && Canvas.GetTop(wall.Rect) + (int)Math.Abs(wall.y2 - wall.y1) > Canvas.GetTop(player) + 50)
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;

                            }
                        } 
                        else if(Canvas.GetLeft(wall.Rect) <= Canvas.GetLeft(player) && (Canvas.GetTop(wall.Rect) < Canvas.GetTop(player)) && Canvas.GetTop(wall.Rect) + (int)Math.Abs(wall.y2 - wall.y1) > Canvas.GetTop(player) + 50) 
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;

                            }
                        }
                        else if (Canvas.GetTop(wall.Rect) > Canvas.GetTop(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;
                                
                            }
                        }
                        else if (Canvas.GetTop(wall.Rect) <= Canvas.GetTop(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;
                  
                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;
               
                            }
                        }
                    } else //HORIZONTAL WALL
                    {
                        if (Canvas.GetTop(wall.Rect) >= Canvas.GetTop(player) && (Canvas.GetLeft(wall.Rect) < Canvas.GetLeft(player)) && Canvas.GetLeft(wall.Rect) + (int)Math.Abs(wall.x2 - wall.x1) > Canvas.GetLeft(player) + 50)
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;
                     
                            }
                        }
                        else if (Canvas.GetTop(wall.Rect) <= Canvas.GetTop(player) && (Canvas.GetLeft(wall.Rect) < Canvas.GetLeft(player)) && Canvas.GetLeft(wall.Rect) + (int)Math.Abs(wall.x2 - wall.x1) > Canvas.GetLeft(player) + 50)
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;
                        
                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;
                   
                            }
                        }
                        else if (Canvas.GetLeft(wall.Rect) < Canvas.GetLeft(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;
                        
                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;
                
                            }
                        }
                        else if (Canvas.GetLeft(wall.Rect) > Canvas.GetLeft(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;
             
                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;
                  
                            }
                        }
                    }
                }
            }

            return true;
        }

        private bool CanMoveBackward(System.Windows.Shapes.Rectangle player, RotateTransform rotateTransform, Rect hitbox)
        {
            foreach (var wall in walls)
            {
                if (hitbox.IntersectsWith(wall.Hitbox))
                {
                    if (wall.isVertical) //VERTIVAL WALL
                    {
                        if (Canvas.GetLeft(wall.Rect) >= Canvas.GetLeft(player) && (Canvas.GetTop(wall.Rect) < Canvas.GetTop(player)) && Canvas.GetTop(wall.Rect) + (int)Math.Abs(wall.y2 - wall.y1) > Canvas.GetTop(player) + 50)
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;

                            }
                        }
                        else if (Canvas.GetLeft(wall.Rect) <= Canvas.GetLeft(player) && (Canvas.GetTop(wall.Rect) < Canvas.GetTop(player)) && Canvas.GetTop(wall.Rect) + (int)Math.Abs(wall.y2 - wall.y1) > Canvas.GetTop(player) + 50)
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;

                            }
                        }
                        else if (Canvas.GetTop(wall.Rect) > Canvas.GetTop(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;

                            }
                        }
                        else if (Canvas.GetTop(wall.Rect) <= Canvas.GetTop(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;

                            }
                        }
                    }
                    else //HORIZONTAL WALL
                    {
                        if (Canvas.GetTop(wall.Rect) >= Canvas.GetTop(player) && (Canvas.GetLeft(wall.Rect) < Canvas.GetLeft(player)) && Canvas.GetLeft(wall.Rect) + (int)Math.Abs(wall.x2 - wall.x1) > Canvas.GetLeft(player) + 50)
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;

                            }
                        }
                        else if (Canvas.GetTop(wall.Rect) <= Canvas.GetTop(player) && (Canvas.GetLeft(wall.Rect) < Canvas.GetLeft(player)) && Canvas.GetLeft(wall.Rect) + (int)Math.Abs(wall.x2 - wall.x1) > Canvas.GetLeft(player) + 50)
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;

                            }
                        }
                        else if (Canvas.GetLeft(wall.Rect) < Canvas.GetLeft(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Right")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Right")
                            {
                                return false;

                            }
                        }
                        else if (Canvas.GetLeft(wall.Rect) > Canvas.GetLeft(player))
                        {
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Bottom Left")
                            {
                                return false;

                            }
                            if (GetRectangleQuadrant(rotateTransform.Angle) == "Top Left")
                            {
                                return false;

                            }
                        }
                    }
                }
            }

            return true;
        }

        private void Die()
        {
            var newWindow = new GameWindow();
            this.Hide();
            this.Close();
            newWindow.Show();

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

        private void ResetPlayers(double angle, double angle2) 
        {
            Canvas.SetLeft(Player1, startXPlayer1);
            Canvas.SetTop(Player1, startYPlayer1);
            rectangleRotatePlayer1.Angle = angle;
            Canvas.SetLeft(Player2, startXPlayer2);
            Canvas.SetTop(Player2, startYPlayer2);
            rectangleRotatePlayer2.Angle = angle2;
        }

        private void RemoveBullets()
        {
            foreach (var bullet in bullets)
            {
                MyCanvas.Children.Remove(bullet.rectangle);
                bulletsToRemove.Add(bullet);
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

                Canvas.SetLeft(newBulletPlayer1, GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 50.0, 50.0, rectangleRotatePlayer1.Angle)[0] - (5 / 2));
                Canvas.SetTop(newBulletPlayer1, GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 50.0, 50.0, rectangleRotatePlayer1.Angle)[1]);

                Rect bulletHitBox = new Rect(Canvas.GetLeft(Player1) + Player1.Width / 2, GetMiddleOfFrontEdge(Canvas.GetLeft(Player1), Canvas.GetTop(Player1), 50.0, 50.0, rectangleRotatePlayer1.Angle)[1], 5, 5);

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

                Canvas.SetLeft(newBulletPlayer2, GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 50.0, 50.0, rectangleRotatePlayer2.Angle)[0] - (5 / 2));
                Canvas.SetTop(newBulletPlayer2, GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 50.0, 50.0, rectangleRotatePlayer2.Angle)[1]);

                Rect bulletHitBox = new Rect(GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 50.0, 50.0, rectangleRotatePlayer2.Angle)[0] - (5 / 2), GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 50.0, 50.0, rectangleRotatePlayer2.Angle)[1], 5, 5);

                MyCanvas.Children.Add(newBulletPlayer2);

                bullets.Add(new Bullet(newBulletPlayer2, bulletHitBox, rectangleRotatePlayer2.Angle, GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[0], GetMiddleOfFrontEdge(Canvas.GetLeft(Player2), Canvas.GetTop(Player2), 30.0, 30.0, rectangleRotatePlayer2.Angle)[1]));
            }
        }

        public double[] GetMiddleOfFrontEdge(double x, double y, double width = 50.0, double height = 50.0, double angle = 0)
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

        private string GetRectangleQuadrant(double angle)
        {
            // Normalize the angle to be between 0 and 360 degrees
            double normalizedAngle = (angle % 360 + 360) % 360;

            // Define the quadrant based on the angle range
            if (normalizedAngle >= 0 && normalizedAngle < 90)
            {
                return "Bottom Right";
            }
            else if (normalizedAngle >= 90 && normalizedAngle < 180)
            {
                return "Bottom Left";
            }
            else if (normalizedAngle >= 180 && normalizedAngle < 270)
            {
                return "Top Left";
            }
            else
            {
                return "Top Right";
            }
        }

        void GenerateMaze()
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
                n = pick(neighbors);
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
