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
    public class TeachersController : Controller
    {
        private AppContext _context;
        private ItemManager _itemManager;

        public class NewTeacherModel
        {
            [Required(ErrorMessage = "Pole Imię jest wymagane.")]
            [MinLength(2, ErrorMessage = "Imię musi mieć przynajmniej 2 znaki.")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
            [MinLength(2, ErrorMessage = "Nazwisko musi mieć przynajmniej 2 znaki.")]
            public string Surname { get; set; }
            public string BarCode { get; set; }
        }

        public TeachersController(AppContext dbContext, ItemManager itemManager)
        {
            _context = dbContext;
            _itemManager = itemManager;
        }

        public IEnumerable<dynamic> Index()
        {
            var teachers = new List<dynamic>();

            foreach (var t in _context.Teachers.ToList())
            {
                teachers.Add(new
                {
                    id = t.Id,
                    name = t.Name,
                    surname = t.Surname,
                    barcode = t.BarCode,
                    borrowedItems = _context.Items.Include(i => i.Events).ThenInclude(e => e.Teacher).ToList().Count(i => _itemManager.GetWhoBorrowed(i)?.Id == t.Id),
                });
            }

            return teachers;
        }

        [HttpGet("[action]")]
        public dynamic Get(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Taki nauczyciel nie istnieje.");

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

            if(_context.Teachers.Any(t => t.Name.ToLower() == model.Name.ToLower() && t.Surname.ToLower() == model.Surname.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim imieniem i nazwiskiem.");

            if (_context.Teachers.Any(t => t.BarCode.ToLower() == model.BarCode.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim kodem kreskowym.");

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

        [HttpPost("[action]/{id}")]
        public IActionResult Update(int id, [FromBody] NewTeacherModel model)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Taki nauczyciel nie istnieje.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Teachers.Any(t => t.Id != teacher.Id && t.Name.ToLower() == model.Name.ToLower() && t.Surname.ToLower() == model.Surname.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim imieniem i nazwiskiem.");

            if (_context.Teachers.Any(t => t.Id != teacher.Id && t.BarCode.ToLower() == model.BarCode.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim kodem kreskowym.");

            if (_context.Items.Any(i => i.ShortId.ToUpper() == model.BarCode.ToUpper()))
                return BadRequest("Istnieje już przedmiot z takim kodem kreskowym.");

            teacher.Name = model.Name;
            teacher.Surname = model.Surname;
            teacher.BarCode = model.BarCode?.ToUpper() ?? "";

            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public IActionResult Remove(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Taki nauczyciel nie istnieje.");

            foreach (var ev in _context.Items.Include(i => i.Events).ThenInclude(e => e.Teacher).SelectMany(i => i.Events))
                if (ev.Teacher?.Id == teacher.Id)
                    ev.Teacher = null;

            _context.Remove(teacher);
            _context.SaveChanges();

            return Ok();
        }
    }
}
