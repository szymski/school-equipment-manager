using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolEquipmentManager.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int ShortId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Location Location { get; set; }
    }
}
