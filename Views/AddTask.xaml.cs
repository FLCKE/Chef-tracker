using Chef_tracker.Models;
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
    /// Logique d'interaction pour AddTask.xaml
    /// </summary>
    public partial class AddTask : UserControl
    {
        public AddTask()
        {
            InitializeComponent();
           
        }
        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les données saisies
            var selectedTask = cmbTask.SelectedItem as TasksMd;
            string task = selectedTask.Name;
            var selectedUser = cmbUser.SelectedItem as User;
            string user = selectedUser.UserName;
            string description = txtDescription.Text;
            DateTime? date = dpDate.SelectedDate;


            // Cast du DataContext dans le type AddProductVM
            var viewModel = DataContext as AddTaskVM;
            if (viewModel != null)
            {
                viewModel.AssignTask(task, user, description, date?.ToString("yyyy-MM-dd"));
                MessageBox.Show($"tache assigné");
            }
            else
            {
                MessageBox.Show("Erreur : ViewModel introuvable !");
            }
        }
    }
}
