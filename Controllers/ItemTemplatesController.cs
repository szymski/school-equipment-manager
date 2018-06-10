using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class ItemTemplatesController : Controller
    {
        public class AddItemTemplateModel
        {
            [Required(ErrorMessage = "Pole Nazwa typu jest wymagane.")]
            [MinLength(2, ErrorMessage = "Minimalna długość nazwy to 2 znaki.")]
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class UpdateNameViewModel
        {
            [Required]
            public int Id { get; set; }
            [MinLength(2, ErrorMessage = "Minimalna długość nazwy to 2 znaki.")]
            public string Name { get; set; }
        }

        public class UpdateDescriptionViewModel
        {
            [Required]
            public int Id { get; set; }
            public string Description { get; set; }
        }

        private AppContext _context;

        public ItemTemplatesController(AppContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<dynamic> Index()
        {
            return _context.ItemTemplates.ToList().Select(t => new
            {
                id = t.Id,
                name = t.Name,
                description = t.Description,
                useCount = _context.Items.Include(i => i.Template).Count(i => i.Template == t),
            });
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] AddItemTemplateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.ItemTemplates.Any(t => t.Name == model.Name))
                return BadRequest("Istnieje już typ o takiej nazwie.");

            _context.ItemTemplates.Add(new ItemTemplate()
            {
                Name = model.Name,
                Description = model.Description
            });
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public IActionResult Remove(int id)
        {
            var template = _context.ItemTemplates.FirstOrDefault(t => t.Id == id);

            if (template == null)
                return BadRequest("Nie ma typu o takim id.");

            foreach (var item in _context.Items.Where(i => i.Template == template))
                item.Template = null;
            _context.ItemTemplates.Remove(template);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult UpdateName([FromBody] UpdateNameViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var template = _context.ItemTemplates.FirstOrDefault(t => t.Id == model.Id);

            if (template == null)
                return BadRequest("Nie ma typu o takim id.");

            if (_context.ItemTemplates.Any(t => t.Name == model.Name))
                return BadRequest("Istnieje już typ o takiej nazwie.");

            template.Name = model.Name;

            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult UpdateDescription([FromBody] UpdateDescriptionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var template = _context.ItemTemplates.FirstOrDefault(t => t.Id == model.Id);

            if (template == null)
                return BadRequest("Nie ma typu o takim id.");

            template.Description = model.Description;

            _context.SaveChanges();

            return Ok();
        }
    }
}
