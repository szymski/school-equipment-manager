using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolEquipmentManager.Models
{
    public class Item
    {
        private string _name;

        public int Id { get; set; }
        public string ShortId { get; set; }
        public string Name
        {
            get => _name ?? Template.Name ?? "INVALID NAME";
            set => _name = value;
        }
        public string Notes { get; set; }
        /// <summary>
        /// Can be null. Then means that the location has to be set yet.
        /// </summary>
        public virtual Location Location { get; set; }
        public virtual ItemTemplate Template { get; set; }
    }
}
