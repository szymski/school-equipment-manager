using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Logic;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class TeachersController : Controller
    {
        public class NewTeacherModel
        {
            [Required(ErrorMessage = "Pole Imię jest wymagane.")]
            [MinLength(2, ErrorMessage = "Imię musi mieć przynajmniej 2 znaki.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
            [MinLength(2, ErrorMessage = "Nazwisko musi mieć przynajmniej 2 znaki.")]
            public string Surname { get; set; }

            public string BarCode { get; set; }

            public bool EnableAccount { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }

        private AppContext _context;
        private ItemManager _itemManager;
        private UserManager<ApplicationUser> _userManager;
        private UserGetter _userGetter;
        private IMessageService _messageService;

        public TeachersController(AppContext dbContext, ItemManager itemManager,
            UserManager<ApplicationUser> userManager, IMessageService messageService, UserGetter userGetter)
        {
            _context = dbContext;
            _itemManager = itemManager;
            _userManager = userManager;
            _messageService = messageService;
            _userGetter = userGetter;
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
                    borrowedItems = _context.Items.Include(i => i.Events).ThenInclude(e => e.Teacher).ToList()
                        .Count(i => _itemManager.GetWhoBorrowed(i)?.Id == t.Id),
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

            var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == teacher.Id);

            if (user == null)
                return new
                {
                    id = teacher.Id,
                    name = teacher.Name,
                    surname = teacher.Surname,
                    barcode = teacher.BarCode,
                    enableAccount = false,
                };

            return new
            {
                id = teacher.Id,
                name = teacher.Name,
                surname = teacher.Surname,
                barcode = teacher.BarCode,
                enableAccount = true,
                username = user.UserName,
                email = user.Email,
            };
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] NewTeacherModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Teachers.Any(t =>
                t.Name.ToLower() == model.Name.ToLower() && t.Surname.ToLower() == model.Surname.ToLower()))
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

            _messageService.SendMessage(teacher,
                "Witaj w systemie ewidencji inwentarzu!",
                "Wejdź do zakładki pomoc by dowiedzieć się jak skorzystać z systemu.");

            return Json(new
            {
                teacherId = teacher.Id,
            });
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NewTeacherModel model)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Taki nauczyciel nie istnieje.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Teachers.Any(t =>
                t.Id != teacher.Id && t.Name.ToLower() == model.Name.ToLower() &&
                t.Surname.ToLower() == model.Surname.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim imieniem i nazwiskiem.");

            if (_context.Teachers.Any(t => t.Id != teacher.Id && t.BarCode.ToLower() == model.BarCode.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim kodem kreskowym.");

            if (_context.Items.Any(i => i.ShortId.ToUpper() == model.BarCode.ToUpper()))
                return BadRequest("Istnieje już przedmiot z takim kodem kreskowym.");

            bool accountCreated = false;
            string generatedPassword = null;

            if (model.EnableAccount)
            {
                if (String.IsNullOrEmpty(model.Username))
                    return BadRequest("Nie podano nazwy użytkownika.");

                if (model.Username.Length < 2)
                    return BadRequest("Nazwa użytkownika musi mieć przynajmniej 2 znaki.");

                var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == teacher.Id);

                // TODO: Check if username or email already exists.

                // Create new user account
                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        Teacher = teacher,
                        UserName = model.Username.Trim(),
                        Email = model.Email.Trim(),
                    };

                    // TODO: Better password generator.
                    generatedPassword = new Random().Next(10000000, 99999999).ToString();

                    await _userManager.CreateAsync(user, generatedPassword);

                    _messageService.SendMessage(teacher,
                        "Nowe hasło",
                        $"Dla twojego konta zostało wygenerowane nowe hasło: <b>{generatedPassword}</b><br>Powinieneś je zmienić w zakładce moje konto.");

                    accountCreated = true;
                }
                // Update user account
                else
                {
                    user.UserName = model.Username.Trim();
                    user.Email = model.Email.Trim();
                }
            }
            else
            {
                var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == teacher.Id);

                // Remove user account if exists
                if (user != null)
                    await _userManager.DeleteAsync(user);
            }

            var oldBarcode = teacher.BarCode;

            teacher.Name = model.Name;
            teacher.Surname = model.Surname;
            teacher.BarCode = model.BarCode?.Trim().ToUpper() ?? "";

            // Send a message that the barcode has been changed
            if (!string.IsNullOrEmpty(teacher.BarCode) && oldBarcode.ToUpper().Trim() != teacher.BarCode)
            {
                _messageService.SendMessage(teacher,
                    "Ustawiono nowy identyfikator dla konta",
                    $"Dla twojego konta został ustawiony nowy identyfikator: {model.BarCode?.Trim().ToUpper()}<br><br>" +
                    $"<img src='/api/BarCode/Generate?text={model.BarCode.Trim().ToUpper()}'/>");
            }

            _context.SaveChanges();

            if (accountCreated)
                return Json(new
                {
                    generatedPassword = generatedPassword,
                });

            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Taki nauczyciel nie istnieje.");

            var loggedUser = _userGetter.GetCurrentUser();
            if (loggedUser.Teacher.Id == teacher.Id)
                return BadRequest("Nie możesz usunąć własnego profilu.");

            foreach (var ev in _context.Items.Include(i => i.Events).ThenInclude(e => e.Teacher)
                .SelectMany(i => i.Events))
                if (ev.Teacher?.Id == teacher.Id)
                    ev.Teacher = null;

            var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == teacher.Id);
            if (user != null)
                await _userManager.DeleteAsync(user);

            _context.Remove(teacher);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Taki nauczyciel nie istnieje.");

            // TODO: Check if the user has rights to do this.

            var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == teacher.Id);
            if (user == null)
                return BadRequest("Ten nauczyciel nie ma aktywowanego konta.");

            var generatedPassword = new Random().Next(10000000, 99999999).ToString();

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, generatedPassword);

            _messageService.SendMessage(teacher,
                "Nowe hasło",
                $"Dla twojego konta zostało wygenerowane nowe hasło: <b>{generatedPassword}</b><br>Powinieneś je zmienić w zakładce moje konto.");

            return Json(new
            {
                generatedPassword = generatedPassword,
            });
        }
    }
}
