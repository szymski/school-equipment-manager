using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        public class AddLocationViewModel
        {
            public string Name { get; set; }
        }

        public class RemoveLocationViewModel
        {
            public int Id { get; set; }
        }

        private AppContext _context;

        public LocationsController(AppContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<dynamic> Index()
        {
            return _context.Locations.Select(l => new
            {
                id = l.Id,
                name = l.Name,
            });
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] ItemsController.AddLocationViewModel model)
        {
            if (_context.Locations.Any(l => l.Name == model.Name))
                return BadRequest("Istnieje już położenie o takiej nazwie");

            _context.Locations.Add(new Location()
            {
                Name = model.Name,
            });
            _context.SaveChanges();

            return Content("ok");
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody] ItemsController.RemoveLocationViewModel model)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == model.Id);

            if (location != null)
            {
                foreach (var item in _context.Items.Where(i => i.Location == location))
                    item.Location = null;
                _context.Locations.Remove(location);
                _context.SaveChanges();
            }

            return Content("ok");
        }
    }
}
