using System.Windows;
using System.Windows.Forms;

namespace Unfancify
{
    /// <summary>
    /// Interaction logic for UnfancifyWindow.xaml
    /// </summary>
    public partial class UnfancifyWindow : Window
    {

        public UnfancifyWindow()
        {
            InitializeComponent();
        }

        public void selectSource_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UnfancifyViewModel vm = (UnfancifyViewModel)unfancifyPanel.DataContext;
                vm.OnBatchUnfancifyClicked(openDialog.SelectedPath);
            }
        }
    }
}