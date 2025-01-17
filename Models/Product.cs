using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Chef_tracker.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime DateExp { get; set; }   
        public byte[] Img { get; set; }
        public int User_Id { get; set; }
        public string Statut {  get; set; }

    }
}
