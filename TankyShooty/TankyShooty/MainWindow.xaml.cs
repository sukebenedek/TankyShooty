using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TankyShooty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class Score
    {
        public static List<int> Scores { get; } = new List<int> { 0, 0 };
    }

    public static class GameData
    {
        public static string Name1 { get; set; }
        public static string Name2 { get; set; }
        public static int Skin1 { get; set; }
        public static int Skin2 { get; set; }

    }
    public partial class MainWindow : Window
    {
        public static List<int> scores = new List<int> { 0, 0 };

        public MainWindow()
        {
            InitializeComponent();
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/img/mainbg.jpg"));
            bg.Stretch = Stretch.Fill;
            this.Background = bg;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            GameWindow gameWindow = new GameWindow();
            gameWindow.Closed += (s, e) => this.Show();
            gameWindow.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            LobbyWindow lobbyWindow = new LobbyWindow();
            lobbyWindow.Closed += (s, e) => this.Show();
            lobbyWindow.Show();
            this.Hide();
        }
    }
}