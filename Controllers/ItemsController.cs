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
            public int Template { get; set; }
            public string Notes { get; set; }
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

        public class UpdateShortIdViewModel
        {
            public int Id { get; set; }
            public string Identifier { get; set; }
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
                id = i.Id,
                shortId = i.ShortId,
                name = i.Name,
                notes = i.Notes,
                description = i.Template != null ? i.Template.Description : "",
                location = i.Location != null ? i.Location.Name : "",
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<dynamic> Events(int id)
        {
            var item = _context.Items.Include(i => i.Events).FirstOrDefault(i => i.Id == id);

            if (item == null)
                return new List<dynamic>();

            List<dynamic> events = new List<dynamic>();

            if (item.Events != null)
                foreach (var ev in item.Events)
                    events.Add(new
                    {
                        id = ev.Id,
                        teacher = ev.Teacher?.Id ?? 0,
                        date = ev.Date,
                        type = ev.Type,
                    });

            return events;
        }


        [HttpPost("[action]")]
        public IActionResult UpdateShortId([FromBody] UpdateShortIdViewModel model)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == model.Id);
            if (item == null)
                return Content("No such item");

            item.ShortId = model.Identifier;
            _context.SaveChanges();

            return Content("ok");
        }

        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
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
            var template = _context.ItemTemplates.FirstOrDefault(l => l.Id == model.Template);

            if (template == null)
                return Content("Invalid template");

            _context.Items.Add(new Item()
            {
                ShortId = null,
                Notes = model.Notes,
                Location = location,
                Template = template,
            });
            _context.SaveChanges();

            return Content("ok");
        }
    }
}
