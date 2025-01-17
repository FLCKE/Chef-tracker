using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Chef_tracker.Models;
using Chef_tracker.Utilities;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Input;

namespace Chef_tracker.ViewsModels
{
     class HomeVM: ViewModelBase
    {

      
        private ObservableCollection<Notifications> _Notif ;

        public ObservableCollection<Notifications> Notif         {
            get { return _Notif; }
            set { _Notif = value; OnPropertyChanged(); }
        }
      //  private NavigationVM _Nav; 
        //public NavigationVM Nav {
          //  get { return _Nav; } 
            //set { _Nav = value; OnPropertyChanged(nameof(_Nav)); }
      //  }
        public HomeVM() {
           

            var  con = new Database_connection();
            
            Notif=con.getAllNotification();
            CheckExpirationAndUpdate();



        }
        public void CheckExpirationAndUpdate()
        {
            var con = new Database_connection(); // Votre classe de connexion
            string selectQuery = @"SELECT ProductId, ProductName, DateExp, Statut FROM Product WHERE DateExp <= @CurrentDate AND Statut != 'Expiré'";
            string updateQuery = @"UPDATE Product SET Statut = 'Expiré' WHERE ProductId = @ProductId";
            string insertNotificationQuery = @"INSERT INTO Notification (product_id, old_status, new_status, description, change_date) 
                                       VALUES (@ProductId, @OldStatus, @NewStatus, @Description, @DateCreated)";

            try
            {
                con._database.Open();

                using (var selectCommand = new SQLiteCommand(selectQuery, con._database))
                {
                    selectCommand.Parameters.AddWithValue("@CurrentDate", DateTime.Now);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Récupérer les informations du produit
                            int productId = reader.GetInt32(0);
                            string productName = reader.GetString(1);
                            DateTime dateExp = reader.GetDateTime(2);
                            string oldStatus = reader.GetString(3);

                            // Mise à jour du statut dans la table Product
                            using (var updateCommand = new SQLiteCommand(updateQuery, con._database))
                            {
                                updateCommand.Parameters.AddWithValue("@ProductId", productId);
                                updateCommand.ExecuteNonQuery();
                            }

                            // Insérer une notification dans la table Notification
                            using (var insertCommand = new SQLiteCommand(insertNotificationQuery, con._database))
                            {
                                insertCommand.Parameters.AddWithValue("@ProductId", productId);
                                insertCommand.Parameters.AddWithValue("@OldStatus", oldStatus);
                                insertCommand.Parameters.AddWithValue("@NewStatus", "Expiré");
                                insertCommand.Parameters.AddWithValue("@Description", $"Votre article N°{productId} '{productName}' est arrivé à expiration.");
                                insertCommand.Parameters.AddWithValue("@DateCreated", DateTime.Now);

                                insertCommand.ExecuteNonQuery();
                            }

                            Debug.WriteLine($"Produit '{productName}' (ID: {productId}) mis à jour en 'Expiré'. Notification créée.");
                        }
                    }
                }
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
