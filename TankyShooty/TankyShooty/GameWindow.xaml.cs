using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        bool moveForwardPlayer1, moveBackwardPlayer1, moveForwardPlayer2, moveBackwardPlayer12 = false;
        int playerSpeed = 10;
        List<Rectangle> itemRemover = new List<Rectangle>();
        List<int> scores = new List<int>(2);

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

            //scoreText.Content = $"{scores[0]} - {scores[1]}";

            if (moveForwardPlayer1 == true)
            {
                Canvas.SetTop(Player1, Canvas.GetTop(Player1) - playerSpeed);
        }
            if(moveBackwardPlayer1 == true)
            {
                Canvas.SetTop(Player1, Canvas.GetTop(Player1) + playerSpeed);
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

            if(e.Key == Key.Space) 
            {
                Rectangle newBulletPlayer1 = new Rectangle
                {
                    Tag = "bulletPlayer1",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red,
                };

                Canvas.SetLeft(newBulletPlayer1, Canvas.GetLeft(Player1) + Player1.Width / 2);
                Canvas.SetTop(newBulletPlayer1, Canvas.GetTop(Player1) - newBulletPlayer1.Height);

                MyCanvas.Children.Add(newBulletPlayer1);
            }
        }
    }
}
