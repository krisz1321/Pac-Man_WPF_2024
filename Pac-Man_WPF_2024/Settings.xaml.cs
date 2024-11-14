using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace Pac_Man_WPF_2024
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public int NumberOfLives { get; set; }
        public int NumberOfGhosts { get; set; }
        public bool horror {  get; set; }
        public GameSettings settings { get; set; }

        public Settings()
        {
            InitializeComponent();
            
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            settings = new GameSettings();
            NumberOfLives = Convert.ToInt32((cb_NumberOfLives.SelectedItem as ComboBoxItem)?.Content.ToString());
            NumberOfGhosts = Convert.ToInt32((cb_NumberOfGhosts.SelectedItem as ComboBoxItem)?.Content.ToString());
            if (cb_Horror.IsChecked == true)
                horror = true;
            else
                horror = false;
            settings.Lives = NumberOfLives;
            settings.GhostCount = NumberOfGhosts;
            settings.Horror = horror;
            this.Close();
        }
    }
}
