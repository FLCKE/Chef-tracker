using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef_tracker.Models
{
    public class Notifications
    {
        public int Notification_id { get; set; }
        public string product_Name { get; set; }
        public string old_status { get; set; }
        public string new_status { get; set; }
        public DateTime change_date { get; set; } 
        public string description { get; set; }

    }
}
