using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public class RemoveItemTemplateModel
        {
            public int Id { get; set; }
        }

        private AppContext _context;

        public ItemTemplatesController(AppContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<dynamic> Index()
        {
            return _context.ItemTemplates.Select(l => new
            {
                id = l.Id,
                name = l.Name,
                description = l.Description
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

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody] RemoveItemTemplateModel model)
        {
            var template = _context.ItemTemplates.FirstOrDefault(l => l.Id == model.Id);

            if (template != null)
            {
                foreach (var item in _context.Items.Where(i => i.Template == template))
                    item.Template = null;
                _context.ItemTemplates.Remove(template);
                _context.SaveChanges();
            }

            return Content("ok");
        }
    }
}
