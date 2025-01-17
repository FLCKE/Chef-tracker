using Chef_tracker.Utilities;
using Chef_tracker.ViewsModels;
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

namespace Chef_tracker.Views
{
    /// <summary>
    /// Logique d'interaction pour home.xaml
    /// </summary>
    /// 
    public partial class home : UserControl
    {
        public ICommand NavigateToPage2Command { get; }
        public home()
        {
            InitializeComponent();
            NavigateToPage2Command = new RelayCommand(NavigateToPage2);

        }
        private void NavigateToPage2(object obj)
        {
            var Nav = new NavigationVM();
            // Mettez à jour CurrentView pour afficher Page2
            Nav.AdminConCommand.Execute(null);
        }
    }
}
