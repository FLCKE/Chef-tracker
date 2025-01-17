using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Chef_tracker.Models;
using System.Collections.ObjectModel;
using Chef_tracker.Utilities;
namespace Chef_tracker.ViewsModels
{
    class AddTaskVM : ViewModelBase
    {
        private ObservableCollection<TasksMd> _tasks; // Nom cohérent pour la variable

        public ObservableCollection<TasksMd> Tasks
        {
            get { return _tasks; }
            set { _tasks = value; OnPropertyChanged(); }
        }
        private ObservableCollection<User> _users; // Nom cohérent pour la variable

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(); }
        }
        public AddTaskVM() { 
            Tasks = new ObservableCollection<TasksMd>();
            AdminUsersVM adUser = new AdminUsersVM();
            if (adUser != null)
            {
                 Users = adUser.users ;
            }
            GetAllTasks();
        }
        public void AssignTask(string task, string user,string description, string date)
        {
            var con = new Database_connection(); // Instanciation de votre classe de connexion
            string query = @"INSERT INTO TaskByUser_New (Task, User, Date, Description, Statut)
                     VALUES ((SELECT task_Id FROM Task WHERE task_name = @taskName), 
                             (SELECT user_Id FROM User WHERE user_name = @memberName), 
                             @date,
                             @description,
                             @status)";

            try
            {
                con._database.Open(); // Ouvrir la connexion à la base de données

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter les paramètres à la requête
                    command.Parameters.AddWithValue("@taskName", task);
                    command.Parameters.AddWithValue("@memberName", user);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@status", "en attente");
                    command.Parameters.AddWithValue("@description", description);

                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine("Produit ajouté avec succès. ");
                        Debug.WriteLine(task);
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

        public void GetAllTasks()
        {
            var con = new Database_connection();
            string query = @"SELECT *
                FROM Task ";

            try
            {
                con._database.Open();

                using (var command = new SQLiteCommand(query, con._database))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tasks.Add(new TasksMd
                            {
                                // Récupération des colonnes de la base de données
                                Id = reader.GetInt16(0), // Product_id
                                Name = reader.GetString(1), // ProductName

                            });
                            Debug.WriteLine(reader.GetString(1));
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
