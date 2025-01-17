using Chef_tracker.Models;
using Chef_tracker.Utilities;
using Chef_tracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace Chef_tracker.ViewsModels
{
    class TasksVM : ViewModelBase
    {
        private ObservableCollection<TaskByUser> _tasks; // Nom cohérent pour la variable

    public ObservableCollection<TaskByUser> TasksData
        {
            get { return _tasks; }
            set { _tasks = value; OnPropertyChanged(); }
        }
        public ICommand DeleteTaskCommand { get; } // Commande pour supprimer une tâche
        public TasksVM()
        {
            TasksData = new ObservableCollection<TaskByUser>();
            DeleteTaskCommand = new RelayCommand(ExecuteDeleteTask);
            GetAllTasks();
        }
        public void OpenNewWindow_Click(object sender, RoutedEventArgs e)
        {
            Validate newWindow = new Validate();
            newWindow.Show();
        }
        private void ExecuteDeleteTask(object parameter)
        {
            // Vérifier si le paramètre est un tableau
            if (parameter is TaskByUser task)
            {
                // Accéder aux propriétés
                string taskName = task.Task;
                string userName = task.User;
                string date = task.Date.ToString("yyyy-MM-dd");
                Debug.WriteLine($"Task: {taskName}, User: {userName} , date : {task.Date}");

               UpdateTask(task.Task, task.User, date);
            }
        }

        public void UpdateTask(string task_Name, string user_name,string Date)
        {
            var con = new Database_connection();
            string query = @"UPDATE TaskByUser_New 
                SET statut = 'Validé'
                WHERE Task = (SELECT task_Id FROM Task WHERE task_Name = @task_Name)
                AND User = (SELECT user_Id FROM User WHERE user_name = @user_name)
                AND Date = @Date";

            try
            {
                con._database.Open();
                
                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter les paramètres à la requête
                    command.Parameters.AddWithValue("@task_Name", task_Name);
                    command.Parameters.AddWithValue("@user_name", user_name);
                    command.Parameters.AddWithValue("@Date", Date);

                    int rowsAffected = command.ExecuteNonQuery();
                    Debug.WriteLine(query, command.Parameters["@Date"]);
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine("Tâche mise à jour avec succès.");
                        this.TasksData.Clear();
                        GetAllTasks();
                    }
                    else
                    {
                        Debug.WriteLine("Aucune tâche mise à jour. Vérifiez les critères.");
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
                            TasksData.Add(new TaskByUser()
                            {
                                // Récupération des colonnes de la base de données
                                Task = reader.GetString(0), // Product_id
                                User = reader.GetString(1), // ProductName
                                Date = reader.GetDateTime(2), // date_Expration
                                Description = reader.GetString(3),// image
                                Statut = reader.GetString(4), // status
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
