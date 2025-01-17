using Chef_tracker.Models;
using Chef_tracker.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Input;
using System.Windows;
using Chef_tracker.Views;

namespace Chef_tracker.ViewsModels
{
   class AdminProductsVM : ViewModelBase
    {
        private ObservableCollection<Product> _products; // Nom cohérent pour la variable

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }
        public ICommand DeleteProductCommand { get; }
        public AdminProductsVM()
        {
            Products = new ObservableCollection<Product>();
            DeleteProductCommand = new RelayCommand(ExecuteDeleteProduct);
            GetAllProduct();
        }
        private void ExecuteDeleteProduct(object parameter)
        {
            // Vérifier si le paramètre est un tableau
            if (parameter is Product product)
            {
                int id = product.ProductId;
                if (DeleteProduct(id))
                {
                    MessageBox.Show($"Produit avec ID {id} supprimé avec succès.");
                }
                else
                {
                    MessageBox.Show($"Produit avec ID {id} n'a pas été supprimer.");

                }
            }
        }
        public Boolean DeleteProduct(int productId)
        {
            var con = new Database_connection();
            string query = "DELETE FROM Product WHERE ProductId = @ProductId;";

            try
            {
                con._database.Open(); // Ouvrir la connexion

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter le paramètre à la requête
                    command.Parameters.AddWithValue("@ProductId", productId);

                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine($"Produit avec ID {productId} supprimé avec succès.");
                        this.Products = new ObservableCollection<Product>();
                        GetAllProduct();
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine($"Aucun produit trouvé avec ID {productId}.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur : " + ex.Message);
                return false;
            }
            finally
            {
                // Fermer la connexion à la base de données
                if (con._database.State == System.Data.ConnectionState.Open)
                {
                    con._database.Close();
                }
            }
        }

        public void GetAllProduct()
        {
            var con = new Database_connection();
            string query = "SELECT * FROM Product;";

            try
            {
                con._database.Open();

                using (var command = new SQLiteCommand(query, con._database))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _products.Add(new Product()
                            {
                                // Récupération des colonnes de la base de données
                                ProductId = reader.GetInt32(0), // Product_id
                                Name = reader.GetString(1), // ProductName
                                DateExp = reader.GetDateTime(2), // date_Expration
                                Img = (byte[])reader["Image"],// image
                                User_Id = reader.GetInt32(4), //  id de l'utilisateur
                                Statut = reader.GetString(5), // status
                            });
                            Debug.WriteLine(reader.GetString(1) + "eeeddfef");
                        }
                    }
                }
                con._database.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                // Fermer la connexion à la base de données
                if (con._database.State == System.Data.ConnectionState.Open)
                {
                    con._database.Close();
                }
            }

        }
    }

}
