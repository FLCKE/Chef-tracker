using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SQLite;
using Chef_tracker.Models;
using System.Collections.ObjectModel;
using Chef_tracker.Utilities;

namespace Chef_tracker.ViewsModels
{
    class AddProductVM : ViewModelBase
    {
        private ObservableCollection<User> _users; // Nom cohérent pour la variable

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(); }
        }
        public AddProductVM()
        {
            AdminUsersVM adUser = new AdminUsersVM();
            if (adUser != null)
            {
                Users = adUser.users;
            }
        }
        public void AddProduct(string productName, string memberName, string expirationDate)
        {
            var con = new Database_connection(); // Instanciation de votre classe de connexion
            string query = @"INSERT INTO Product (ProductName, user_id, DateExp, statut, Image)
                     VALUES (@productName, 
                             (SELECT user_Id FROM User WHERE user_name = @memberName), 
                             @expirationDate, 
                             @status, 
                             @imagePath)";

            try
            {
                con._database.Open(); // Ouvrir la connexion à la base de données

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter les paramètres à la requête
                    command.Parameters.AddWithValue("@productName", productName);
                    command.Parameters.AddWithValue("@memberName", "Louis");
                    command.Parameters.AddWithValue("@expirationDate", expirationDate);
                    command.Parameters.AddWithValue("@status","actif");
                    command.Parameters.AddWithValue("@imagePath", "89504E470D0A1A0A0000000D49484452");

                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine("Produit ajouté avec succès.");
                    }
                    else
                    {
                        Debug.WriteLine("Aucun produit ajouté. Vérifiez les données.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                if (con._database.State == System.Data.ConnectionState.Open)
                {
                    con._database.Close(); // Fermer la connexion
                }
            }
        }


    }
}
