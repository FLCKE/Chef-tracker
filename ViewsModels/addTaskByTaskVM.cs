using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Chef_tracker.ViewsModels
{
     class addTaskByTaskVM
    {
        public void AddTaskByTask(string name)
        {
            var con = new Database_connection(); // Instanciation de votre classe de connexion
            string query = @"INSERT INTO Task (Task_name) 
                        VALUES (@name)";

            try
            {
                con._database.Open(); // Ouvrir la connexion à la base de données

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter les paramètres à la requête
                    command.Parameters.AddWithValue("@name", name);


                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine("Tâche  ajouté avec succès.");
                    }
                    else
                    {
                        Debug.WriteLine("Aucune tâche ajouté. Vérifiez les données.");
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
