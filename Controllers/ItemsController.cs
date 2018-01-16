using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        public IEnumerable<Item> Index()
        {
            List<Item> items = new List<Item>();

            items.Add(new Item()
            {
                Id = 2137,
                Name = "Myszek komputerowy",
                Description = "Świetny do machania i śpiewania 'Mortadela, mortadela...'",
                Location = "Chuj wie",
            });

            items.Add(new Item()
            {
                Id = 1337,
                Name = "Monitor",
                Description = "Nie działa",
                Location = "Sala 8",
            });

            items.Add(new Item()
            {
                Id = 45,
                Name = "Kabel RJ-45",
                Description = "",
                Location = "Nie wiadomo",
            });

            return items;
        }
    }
}
