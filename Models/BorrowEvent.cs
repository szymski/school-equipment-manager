using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolEquipmentManager.Models
{
    public class BorrowEvent
    {
        public int Id { get; set; }
        public virtual Item Item { get; set; }
        /// <summary>
        /// This field can be null, when the teacher has been removed.
        /// </summary>
        public virtual Teacher Teacher { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }
}
