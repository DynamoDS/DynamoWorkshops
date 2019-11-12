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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RapidFire.UI
{
    /// <summary>
    /// Interaction logic for RapidFireView.xaml
    /// </summary>
    public partial class RapidFireView : Window
    {
        public RapidFireView(RapidFireVM vm)
        {
            InitializeComponent();

            this.DataContext = vm;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
       
    }
}
