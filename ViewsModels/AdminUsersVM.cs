using Chef_tracker.Models;
using Chef_tracker.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Data.SQLite;

namespace Chef_tracker.ViewsModels
{
    class AdminUsersVM : ViewModelBase
    {
        private ObservableCollection<User> _users; // Nom cohérent pour la variable

        public ObservableCollection<User> users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(); }
        }
        public ICommand DeleteUserCommand { get; }
        public AdminUsersVM()
        {
            users = new ObservableCollection<User>();
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser);
            GetAllUsers();
        }
        private void ExecuteDeleteUser(object parameter)
        {
            // Vérifier si le paramètre est un tableau
            if (parameter is User user)
            {
                int id = user.Id;
                if (DeleteUser(id))
                {
                    MessageBox.Show($"Utilisateur avec ID {id} supprimé avec succès.");
                }
                else
                {
                    MessageBox.Show($"Utilisateur avec ID {id} n'a pas été supprimer.");

                }
            }
        }
        public Boolean DeleteUser(int userId)
        {
            var con = new Database_connection();
            string query = "DELETE FROM User WHERE User_Id = @UserId;";

            try
            {
                con._database.Open(); // Ouvrir la connexion

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter le paramètre à la requête
                    command.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine($"Utilisateur avec ID {userId} supprimé avec succès.");
                        this.users = new ObservableCollection<User>();
                        GetAllUsers();
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine($"Aucun utilisateur trouvé avec ID {userId}.");
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

        public void GetAllUsers()
        {
            var con = new Database_connection();
            string query = "SELECT * FROM User;";

            try
            {
                con._database.Open();

                using (var command = new SQLiteCommand(query, con._database))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _users.Add(new User()
                            {
                                // Récupération des colonnes de la base de données
                                Id = reader.GetInt32(0), // User_id
                                UserName = reader.GetString(1), // UserName
                                Email = reader.GetString(2), // Email
                                Role = reader.GetString(4), // Role
                            });

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
