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
    public class TeachersController : Controller
    {
        private AppContext _context;

        public class NewTeacherModel
        {
            [Required]
            [MinLength(2)]
            public string Name { get; set; }
            [Required]
            [MinLength(2)]
            public string Surname { get; set; }
            public string BarCode { get; set; }
        }

        public TeachersController(AppContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<dynamic> Index()
        {
            return _context.Teachers.Select(t => new
            {
                id = t.Id,
                name = t.Name,
                surname = t.Surname,
                barcode = t.BarCode,
            });
        }

        [HttpGet("[action]")]
        public dynamic Get(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return "No such teacher";

            return new
            {
                id = teacher.Id,
                name = teacher.Name,
                surname = teacher.Surname,
                barcode = teacher.BarCode,
            };
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] NewTeacherModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teacher = new Teacher()
            {
                Name = model.Name,
                Surname = model.Surname,
                BarCode = model.BarCode,
            };

            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            return Json(teacher);
        }
    }
}
