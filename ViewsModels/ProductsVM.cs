using Chef_tracker.Models;
using Chef_tracker.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Data.SQLite;

namespace Chef_tracker.ViewsModels
{
    class ProductsVM : ViewModelBase
    {
        private ObservableCollection<Product> _products; // Nom cohérent pour la variable

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        public ProductsVM()
        {
            Products = new ObservableCollection<Product>();
            GetAllProduct();
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
