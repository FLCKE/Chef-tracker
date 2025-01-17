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
    /// Logique d'interaction pour AddProduct.xaml
    /// </summary>
    public partial class AddProduct : UserControl
    {
        public AddProduct()
        {
            InitializeComponent();
            DataContext = new AddProductVM();
        }
        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les données saisies
            string nom = txtNom.Text;
            string membre = cmbMembre.Text;
            DateTime? expiration = dpExpiration.SelectedDate;

            // Cast du DataContext dans le type AddProductVM
            var viewModel = DataContext as AddProductVM;
            if (viewModel != null)
            {
                viewModel.AddProduct(nom, membre, expiration?.ToString("yyyy-MM-dd"));
                MessageBox.Show($"Produit ajouté :\nNom: {nom}\nMembre: {membre}\nExpiration: {expiration}");
            }
            else
            {
                MessageBox.Show("Erreur : ViewModel introuvable !");
            }
        }
    }
}
