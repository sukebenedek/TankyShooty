using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TankyShooty
{
    public partial class LobbyWindow : Window
    {
        private List<string> ImagePaths { get; set; } = new List<string>();
        private int Player1Index { get; set; } = 0;
        private int Player2Index { get; set; } = 0;

        public LobbyWindow()
        {
            InitializeComponent();
            LoadImages();
            UpdateImages();
        }

        private void LoadImages()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/img/" + "/players/", "*.jpg")
                                      .Concat(Directory.GetFiles(Directory.GetCurrentDirectory() + "/img/" + "/players/", "*.jpg"))
                                      .ToArray();

            ImagePaths = files.ToList();
        }

        private void UpdateImages()
        {
            if (ImagePaths.Count > 0)
            {
                Player1Img.Source = new BitmapImage(new Uri(ImagePaths[Player1Index]));
                Player2Img.Source = new BitmapImage(new Uri(ImagePaths[Player2Index]));
            }
        }

        private void Previous_Click1(object sender, RoutedEventArgs e)
        {
            if (Player1Index > 0) Player1Index--;
            else Player1Index = ImagePaths.Count - 1;
            UpdateImages();
        }

        private void Next_Click1(object sender, RoutedEventArgs e)
        {
            if (Player1Index < ImagePaths.Count - 1) Player1Index++;
            else Player1Index = 0;
            UpdateImages();
        }

        private void Previous_Click2(object sender, RoutedEventArgs e)
        {
            if (Player2Index > 0) Player2Index--;
            else Player2Index = ImagePaths.Count - 1;
            UpdateImages();
        }

        private void Next_Click2(object sender, RoutedEventArgs e)
        {
            if (Player2Index < ImagePaths.Count - 1) Player2Index++;
            else Player2Index = 0;
            UpdateImages();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {

            if (Player_1.Text == "" || Player_2.Text == "")
            {
                MessageBox.Show("Adjátok meg játékosneveteket", "Nincs Játékosnév", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (Player_1.Text == Player_2.Text)
            {
                MessageBox.Show("Kérjük adjatok meg különböző játékosneveteket", "Azonos Játékosnév", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (Player1Index == Player2Index)
            {
                MessageBox.Show("Kérjük válasszatok különböző karaktereket", "Azonos Karakter", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                GameWindow gameWindow = new GameWindow();
                gameWindow.Closed += (s, e) => this.Show();
                gameWindow.Show();
                this.Hide();

                string filePath = "nevek.txt";
                string content = $"{Player_1.Text};{Player1Index+1}.jpg\n{Player_2.Text};{Player2Index+1}.jpg";
                File.WriteAllText(filePath, content);

                GameData.Name1 = Player_1.Text;
                GameData.Name2 = Player_2.Text;
                GameData.Skin1 = ++Player1Index;
                GameData.Skin2 = ++Player2Index;

            }

        }
    }
}
