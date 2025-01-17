using Chef_tracker.Models;
using Chef_tracker.Utilities;
using Chef_tracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data.SQLite;
namespace Chef_tracker.ViewsModels
{
    class TaskByTaskVM : ViewModelBase
    {
        private ObservableCollection<TasksMd> _tasksByTask; // Nom cohérent pour la variable

        public ObservableCollection<TasksMd> tasksByTask
        {
            get { return _tasksByTask; }
            set { _tasksByTask = value; OnPropertyChanged(); }
        }
        public ICommand DeleteTaskByTaskCommand { get; }


        public TaskByTaskVM()
        {
            
            tasksByTask = new ObservableCollection<TasksMd>();
           
            DeleteTaskByTaskCommand = new RelayCommand(ExecuteDeleteTaskByTask);
          
            GetAllTasksByTask();
        }

        private void ExecuteDeleteTaskByTask(object parameter)
        {
            // Vérifier si le paramètre est un tableau
            if (parameter is TasksMd task)
            {
                int taskId = task.Id;
                if (DeleteTaskByTask(taskId))
                {
                    MessageBox.Show($"Tache supprimé avec succès.");
                }
                else
                {
                    MessageBox.Show($"Tache n'a pas été supprimer.");

                }
            }
        }

        public void GetAllTasksByTask()
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
                            tasksByTask.Add(new TasksMd()
                            {
                                // Récupération des colonnes de la base de données
                                Id = reader.GetInt16(0),
                                Name = reader.GetString(1),

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
        public Boolean DeleteTaskByTask(int Id)
        {
            var con = new Database_connection();
            string query = "DELETE  FROM Task WHERE task_Id = @taskId";

            try
            {
                con._database.Open(); // Ouvrir la connexion

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter le paramètre à la requête
                    command.Parameters.AddWithValue("@taskId", Id);

                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine($"Tache supprimé avec succès.");
                        this.tasksByTask = new ObservableCollection<TasksMd>();
                        GetAllTasksByTask();
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
    }
}
