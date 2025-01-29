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
        public GameWindow()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            MyCanvas.Focus();
        }

        private void GameLoop(object sender, EventArgs e)
        {

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
