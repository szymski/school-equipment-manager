using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Logic;
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
            public int Number { get; set; }
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

        public class AddEventViewModel
        {
            [Required(ErrorMessage = "Id przedmiotu jest wymagane.")]
            public int Id { get; set; }
            [Required(ErrorMessage = "Nauczyciel jest wymagany.")]
            public int TeacherId { get; set; }
            [Required(ErrorMessage = "Typ zdarzenia jest wymagany")]
            public string Type { get; set; }
        }

        private AppContext _context;
        private ItemManager _itemManager;

        public ItemsController(AppContext dbContext, ItemManager itemManager)
        {
            _context = dbContext;
            _itemManager = itemManager;
        }

        public IEnumerable<dynamic> Index()
        {
            List<dynamic> result = new List<dynamic>();

            foreach (var i in _context.Items.Include(j => j.Location).Include(j => j.Template).Include(j => j.Events))
                result.Add(new
                {
                    id = i.Id,
                    shortId = i.ShortId,
                    name = i.Name,
                    notes = i.Notes,
                    description = i.Template != null ? i.Template.Description : "",
                    location = i.Location != null ? i.Location.Name : "",
                    returned = _itemManager.HasBeenReturned(i),
                });

            return result;
        }

        [HttpGet("[action]")]
        public dynamic Get(int id)
        {
            var item = _context.Items.Include(i => i.Location).FirstOrDefault(i => i.Id == id);

            if (item == null)
                return "No such item";

            return new
            {
                id = item.Id,
                shortId = item.ShortId,
                name = item.Name,
                notes = item.Notes,
                description = item.Template != null ? item.Template.Description : "",
                location = item.Location != null ? item.Location.Name : "",
            };
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<dynamic> Events(int id)
        {
            var item = _context.Items.Include(i => i.Events).Include("Events.Teacher").FirstOrDefault(i => i.Id == id);

            if (item == null)
                return new List<dynamic>();

            List<dynamic> events = new List<dynamic>();

            if (item.Events != null)
                foreach (var ev in item.Events.OrderBy(e => e.Date))
                    events.Add(new
                    {
                        id = ev.Id,
                        teacher = ev.Teacher?.Id ?? -1,
                        date = ev.Date.ToString("G"),
                        type = ev.Type,
                    });

            return events;
        }

        [HttpPost("[action]")]
        public IActionResult AddEvent([FromBody] AddEventViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = _context.Items.Include(i => i.Events).FirstOrDefault(i => i.Id == model.Id);
            if (item == null)
                return BadRequest("Nie ma przedmiotu o takim id.");

            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == model.TeacherId);
            if (teacher == null)
                return BadRequest("Nie ma nauczyciela o takim id.");

            if (model.Type.ToLower() != "borrow" && model.Type.ToLower() != "return")
                return BadRequest($"Nieprawidłowy typ zdarzenia - {model.Type}");

            item.Events.Add(new BorrowEvent()
            {
                Date = DateTime.Now,
                Teacher = teacher,
                Type = model.Type.ToLower(),
            });
            _context.SaveChanges();

            // TODO: Disallow two borrow events without return.

            return Ok();
        }


        [HttpPost("[action]")]
        public IActionResult UpdateShortId([FromBody] UpdateShortIdViewModel model)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == model.Id);
            if (item == null)
                return BadRequest("Nie ma przedmiotu o takim id.");

            if (_context.Items.Any(i => i.Id != item.Id && i.ShortId == model.Identifier))
                return BadRequest("Istnieje już przedmiot z takim identyfikatorem.");

            item.ShortId = model.Identifier;
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return Content("No such item");

            _context.Remove(item);
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

            if (model.Number > 100)
                return Content("Can't add more than 100 items at once");

            for (int i = 0; i < model.Number; i++)
            {
                _context.Items.Add(new Item()
                {
                    ShortId = null,
                    Notes = model.Notes,
                    Location = location,
                    Template = template,
                });
            }
            _context.SaveChanges();

            return Content("ok");
        }
    }
}
