using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolEquipmentManager.Models
{
    public class Message
    {
        public int Id { get; set; }
        public Teacher Recipent { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Read { get; set; }
    }
}
