using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace TankyShooty
{
    /// <summary>
    /// Interaction logic for LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : Window
    {
        private List<string> imagePaths = new List<string>();
        private int player_1_Index = 0;
        private int player_2_Index = 0;
        private string imageFolderPath = @"img/";
        public LobbyWindow()
        {
            InitializeComponent();
            LoadImages();
        }

        private void LoadImages()
        {
            if (Directory.Exists(imageFolderPath))
            {
                // Get all image files from the folder
                string[] files = Directory.GetFiles(imageFolderPath, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))
                    {
                        imagePaths.Add(file);
                    }
                }
                if (imagePaths.Count > 0) DisplayPlayer1(player_1_Index);
                if (imagePaths.Count > 1) DisplayPlayer2(player_2_Index);
            }
            else
            {
                MessageBox.Show("Image folder not found!");
            }
        }

        private void DisplayPlayer1(int index)
        {
            if (index >= 0 && index < imagePaths.Count)
            {
                PlayerImg1.Source = new BitmapImage(new Uri(imagePaths[index]));
                player_1_Index = index;
            }
        }

        private void DisplayPlayer2(int index)
        {
            if (index >= 0 && index < imagePaths.Count)
            {
                PlayerImg2.Source = new BitmapImage(new Uri(imagePaths[index]));
                player_2_Index = index;
            }
        }

        private void Previous_Click1(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                DisplayImage(currentIndex - 1);
            }
        }

        private void Next_Click1(object sender, RoutedEventArgs e)
        {
            if (currentIndex < imagePaths.Count - 1)
            {
                DisplayImage(currentIndex + 1);
            }
        }
        private void Previous_Click2(object sender, RoutedEventArgs e)
        {
            if (CarouselListBox2.SelectedIndex > 0)
                CarouselListBox2.SelectedIndex--;
        }

        private void Next_Click2(object sender, RoutedEventArgs e)
        {
            if (CarouselListBox2.SelectedIndex < Images.Count - 1)
                CarouselListBox2.SelectedIndex++;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            this.Hide();
            gameWindow.Show();
        }
    }
}
