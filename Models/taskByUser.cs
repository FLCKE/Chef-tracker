using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef_tracker.Models
{
    public class TaskByUser
    {
        public String Task { get; set; }
        public String User { get; set; }
        public DateTime Date { get; set; }  
        public String Description { get; set; }
        public string Statut {  get; set; }
    }
}
