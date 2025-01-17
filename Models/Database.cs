using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Chef_tracker.Models
{
    public class Database
    {
        public SQLiteConnection Connection { get; set; }
    }
}