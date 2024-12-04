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


namespace Kyrsuch
{
    public partial class CitiesWindow : Window
    {
        public CitiesWindow()
        {
            InitializeComponent();
            this.DataContext = new CitiesViewModel(); // Підключаємо ViewModel
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CitiesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

