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

namespace DynamoDev.Stats
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class InputsWindow : Window
    {
        public InputsWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
