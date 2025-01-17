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
    /// Logique d'interaction pour AdminCon.xaml
    /// </summary>
    public partial class AdminCon : UserControl
    {
        public AdminCon()
        {
            InitializeComponent();
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les données saisies
            string email = user_mail.Text;
            string  pwd = password.Password;

            // Cast du DataContext dans le type AddProductVM
            var viewModel = DataContext as AdminConVM;
            if (viewModel != null)
            {
               if (viewModel.AuthenticateUser(email, pwd))
                {
                    var navigationVM = Application.Current.MainWindow.DataContext as NavigationVM;
                    if (navigationVM != null)
                    {
                        navigationVM.CurrentView = new AdminHomeVM(); // Remplacez par votre ViewModel cible
                    }
                    MessageBox.Show($"Bienvenue {email} ");
                }
               else
                {
                    MessageBox.Show($"Email ou mot de passe incorrect");
                }
            }
            else
            {
                MessageBox.Show("Erreur : ViewModel introuvable !");
            }
        }
    }
}
