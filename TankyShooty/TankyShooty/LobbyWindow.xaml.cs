using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public LobbyWindow()
        {
            InitializeComponent();
            Images = new ObservableCollection<string>
            {
                "https://via.placeholder.com/200x150/ff7f7f/333333?text=1",
                "https://via.placeholder.com/200x150/7f7fff/333333?text=2",
                "https://via.placeholder.com/200x150/7fff7f/333333?text=3",
                "https://via.placeholder.com/200x150/ff7fff/333333?text=4"
            };

            DataContext = this;
        }
        public ObservableCollection<string> Images { get; set; }

        //private void Previous_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CarouselListBox.SelectedIndex > 0)
        //        CarouselListBox.SelectedIndex--;
        //}

        //private void Next_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CarouselListBox.SelectedIndex < Images.Count - 1)
        //        CarouselListBox.SelectedIndex++;
        //}
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }
    }
}
