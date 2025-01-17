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
using Chef_tracker.Views;

namespace Chef_tracker.ViewsModels
{
    class AdminTaskVM : ViewModelBase
    {
        private ObservableCollection<TaskByUser> _tasks; // Nom cohérent pour la variable

        public ObservableCollection<TaskByUser> tasks
        {
            get { return _tasks; }
            set { _tasks = value; OnPropertyChanged(); }
        }
        public ICommand DeleteTaskCommand { get; }
        

        public AdminTaskVM()
        {
            tasks = new ObservableCollection<TaskByUser>();
          
            DeleteTaskCommand = new RelayCommand(ExecuteDeleteTask);
          
            GetAllTasks();
           
        }
        private void ExecuteDeleteTask(object parameter)
        {
            // Vérifier si le paramètre est un tableau
            if (parameter is TaskByUser task)
            {
                string taskName = task.Task;
                string userName = task.User;
                string date = task.Date.ToString("yyyy-MM-dd");
                if (DeleteTask(taskName,userName,date))
                {
                    MessageBox.Show($"Tache supprimé avec succès.");
                }
                else
                {
                    MessageBox.Show($"Tache n'a pas été supprimer.");

                }
            }
        }   
      
        public Boolean DeleteTask(string taskName, string userName, string date)
        {
            var con = new Database_connection();
            string query = "DELETE FROM TaskByUser_New WHERE Task = (SELECT task_Id FROM Task WHERE task_Name = @task_Name) AND User = (SELECT user_Id FROM User WHERE user_name = @user_name) AND Date = @Date";

            try
            {
                con._database.Open(); // Ouvrir la connexion

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter le paramètre à la requête
                    command.Parameters.AddWithValue("@task_Name", taskName);
                    command.Parameters.AddWithValue("@user_name", userName);
                    command.Parameters.AddWithValue("@Date", date);

                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine($"Tache supprimé avec succès.");
                        this.tasks = new ObservableCollection<TaskByUser>();
                        GetAllTasks();
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine($"Aucun tâches trouvé avec ID .");
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


        public void GetAllTasks()
        {
            var con = new Database_connection();
            string query = @"SELECT A.task_Name, U.user_name, T.Date, T.Description, T.Statut
                FROM TaskByUser_New T
                JOIN User U ON U.user_Id = T.User
                JOIN Task A ON A.task_Id = T.Task
                WHERE T.statut != 'Validé'";

            try
            {
                con._database.Open();

                using (var command = new SQLiteCommand(query, con._database))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskByUser()
                            {
                                // Récupération des colonnes de la base de données
                                Task = reader.GetString(0), 
                                User = reader.GetString(1),
                                Date = reader.GetDateTime(2),
                                Description = reader.GetString(3),
                                Statut = reader.GetString(4), 
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

