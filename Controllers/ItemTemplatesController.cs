using System;
using System.Collections.Generic;
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
            _context.ItemTemplates.Add(new ItemTemplate()
            {
                Name = model.Name,
                Description = model.Description
            });
            _context.SaveChanges();

            return Content("ok");
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
