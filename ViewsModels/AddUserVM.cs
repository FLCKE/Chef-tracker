using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
namespace Chef_tracker.ViewsModels
{
    class AddUserVM
    {
        public void AddUser(string name, string email, string role,string pwd)
        {
            var con = new Database_connection(); // Instanciation de votre classe de connexion
            string query = @"INSERT INTO User (user_name, email, password, role) 
                        VALUES (@name,
                             @email, 
                             @password, 
                             @role)";

            try
            {
                con._database.Open(); // Ouvrir la connexion à la base de données

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter les paramètres à la requête
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email",email);
                    command.Parameters.AddWithValue("@password", pwd);
                    command.Parameters.AddWithValue("@role", role);
           

                    int rowsAffected = command.ExecuteNonQuery(); // Exécuter la requête
                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine("utilisateur ajouté avec succès.");
                    }
                    else
                    {
                        Debug.WriteLine("Aucun utilisateur ajouté. Vérifiez les données.");
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
