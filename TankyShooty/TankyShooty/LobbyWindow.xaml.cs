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
        private string imageFolderPath = System.IO.Path.GetFullPath("img/");
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
                
                CarouselListBox1.ItemsSource = imagePaths;
                CarouselListBox2.ItemsSource = imagePaths;

                if (imagePaths.Count > 0) 
                {
                    CarouselListBox1.SelectedIndex = player_1_Index;
                    CarouselListBox2.SelectedIndex = player_2_Index;
                }
            }
        }

        private void DisplayPlayer1(int index)
        {
            if (index >= 0 && index < imagePaths.Count)
            {
               // imgPlayer_1.Source = new BitmapImage(new Uri(imagePaths[index], UriKind.Absolute));
                player_1_Index = index;
            }
        }

        private void DisplayPlayer2(int index)
        {
            if (index >= 0 && index < imagePaths.Count)
            {
                CarouselListBox2.SelectedIndex = index;
                player_2_Index = index;
            }
        }

        private void Previous_Click1(object sender, RoutedEventArgs e)
        {
            if (player_1_Index > 0)
            {
                DisplayPlayer1(player_1_Index - 1);
            }
        }

        private void Next_Click1(object sender, RoutedEventArgs e)
        {
            if (player_1_Index < imagePaths.Count - 1)
            {
                DisplayPlayer1(player_1_Index + 1);
            }
        }
        private void Previous_Click2(object sender, RoutedEventArgs e)
        {
            if (player_2_Index > 0)
            {
                DisplayPlayer2(player_2_Index - 1);
            }
        }

        private void Next_Click2(object sender, RoutedEventArgs e)
        {
            if (player_2_Index < imagePaths.Count - 1)
            {
                DisplayPlayer2(player_2_Index + 1);
            }
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {
            LobbyWindow lobbyWindow = new LobbyWindow();
            GameWindow gameWindow = new GameWindow();
            gameWindow.Closed += (s, e) => this.Show();
            gameWindow.Show();
            this.Hide();
            string filePath = "nevek.txt";

            string content = $"{Player_1.Text};{Player_2.Text}";

            File.WriteAllText(filePath, content);
        }
    }
}
