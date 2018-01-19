using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        public class NewItemViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Location { get; set; }
        }

        public class AddLocationViewModel
        {
            public string Name { get; set; }
        }

        public class RemoveLocationViewModel
        {
            public int Id { get; set; }
        }

        private AppContext _context;

        public ItemsController(AppContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<dynamic> Index()
        {
            return _context.Items.Select(i => new
            {
                id = i.ShortId,
                name = i.Name,
                description = i.Description,
                location = i.Location != null ? i.Location.Name : "",
            });
        }

        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.ShortId == id);
            if (item == null)
                return Content("No such item");

            _context.Items.Remove(item);
            _context.SaveChanges();

            return Content("ok");
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] NewItemViewModel model)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == model.Location);

            _context.Items.Add(new Item()
            {
                ShortId = new Random().Next(100, 10000),
                Name = model.Name,
                Description = model.Description,
                Location = location,
            });
            _context.SaveChanges();

            return Content("ok");
        }
    }
}
