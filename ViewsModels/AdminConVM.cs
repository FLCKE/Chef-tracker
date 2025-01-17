using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Chef_tracker.ViewsModels
{
   class AdminConVM
    {
        public bool AuthenticateUser(string email, string password)
        {
            var con = new Database_connection(); // Instanciation de votre classe de connexion
            string query = @"SELECT COUNT(1) 
                     FROM User 
                     WHERE email = @Email AND password = @Password AND role IN ('manager', 'admin')";

            try
            {
                con._database.Open(); // Ouvrir la connexion à la base de données

                using (var command = new SQLiteCommand(query, con._database))
                {
                    // Ajouter les paramètres à la requête
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    // Exécuter la requête et vérifier si une correspondance existe
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0; // Renvoie true si au moins une correspondance est trouvée
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur : " + ex.Message);
                return false; // Renvoie false en cas d'erreur
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
