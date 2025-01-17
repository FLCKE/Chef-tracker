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
    /// Logique d'interaction pour AddTaskByTask.xaml
    /// </summary>
    public partial class AddTaskByTask : UserControl
    {
        public AddTaskByTask()
        {
            InitializeComponent();
        }
        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les données saisies
            string nom = txtTask.Text;

            // Cast du DataContext dans le type AddProductVM
            var viewModel = DataContext as addTaskByTaskVM ;
            if (viewModel != null)
            {
                viewModel.AddTaskByTask(nom);
                MessageBox.Show($"Utilisateur  ajouté :\nNom: {nom}");
            }
            else
            {
                MessageBox.Show("Erreur : ViewModel introuvable !");
            }
        }
    }
}
