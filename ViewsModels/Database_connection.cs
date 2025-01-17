using Chef_tracker.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;

namespace Chef_tracker.ViewsModels
{
    public class Database_connection
    {
        public SQLiteConnection _database;

        public Database_connection()
        {
            // Initialisation de la connexion à la base de données
            this._database = new SQLiteConnection("Data Source=../../database/chefTrackerDb.db;Version=3;");
        }

        public ObservableCollection<Notifications> getAllNotification()
        {
            ObservableCollection<Notifications> _notif = new ObservableCollection<Notifications>();
            string query = @"
            SELECT N.notification_id, P.ProductName, N.old_status, N.new_status, N.change_date, N.description
            FROM Notification N
            JOIN Product P ON N.product_id = P.ProductId
            ORDER BY N.change_date DESC;
            ";

            try
            {
                // Ouvrir la connexion à la base de données
                this._database.Open();
                Debug.WriteLine("connexion activé");

                using (var command = new SQLiteCommand(query, this._database))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        // Lecture des résultats et ajout à la collection
                        while (reader.Read())
                        {
                            _notif.Add(new Notifications()
                            {
                                // Récupération des colonnes depuis la base de données
                                Notification_id = reader.GetInt32(0), // notification_id
                                product_Name = reader.GetString(1), // ProductName
                                old_status = reader.GetString(2), // old_status
                                new_status = reader.GetString(3), // new_status
                                change_date = reader.GetDateTime(4), // change_date
                                description = reader.IsDBNull(5) ? "Aucune description" : reader.GetString(5), // description
                            });
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher le message d'erreur dans la console
                Debug.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                // Fermer la connexion à la base de données
                if (this._database.State == System.Data.ConnectionState.Open)
                {
                    this._database.Close();
                }
            }

            return _notif;
        }
    }
}
