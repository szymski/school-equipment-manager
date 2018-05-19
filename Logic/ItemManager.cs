using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public class ItemManager
    {
        public bool HasBeenReturned(Item item)
        {
            if (item.Events == null)
                return true;

            var lastEvent = item.Events.Where(e => e.Type == "borrow" || e.Type == "return").OrderByDescending(e => e.Date)?.FirstOrDefault();
            bool returned = !(lastEvent != null && lastEvent.Type == "borrow");

            return returned;
        }

        public Teacher GetWhoBorrowed(Item item)
        {
            var lastEvent = item.Events?.Where(e => e.Type == "borrow" || e.Type == "return").OrderByDescending(e => e.Date)?.FirstOrDefault();
            if (lastEvent == null)
                return null;

            if (lastEvent.Type == "borrow")
                return lastEvent.Teacher;

            return null;
        }
    }
}
