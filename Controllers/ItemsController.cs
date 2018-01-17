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
                location = i.Location,
            });
        }

        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            _context.Items.Remove(_context.Items.FirstOrDefault(i => i.ShortId == id));
            _context.SaveChanges();
            return Content("ok");
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] NewItemViewModel model)
        {
            _context.Items.Add(new Item()
            {
                ShortId = new Random().Next(100, 10000),
                Name = model.Name,
                Description = model.Description,
                Location = "",
            });
            _context.SaveChanges();

            return Content("ok");
        }
    }
}
