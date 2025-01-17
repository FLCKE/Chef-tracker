using Chef_tracker.Models;
using Chef_tracker.ViewsModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chef_tracker.Views
{
    /// <summary>
    /// Logique d'interaction pour Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        public Tasks()
        {
            InitializeComponent();
        }
         private void OpenNewWindow_Click(object sender, RoutedEventArgs e)
        {
            Validate newWindow = new Validate();
            newWindow.Show();
        }
    }
}
