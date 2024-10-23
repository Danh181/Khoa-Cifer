using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace Music_Player
{

    public partial class MainWindow : Window
    {
        string[] paths, files;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void QuitButton_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you really want to quit??", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (answer == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void GODS_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}