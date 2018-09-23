using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Logic;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
            public string Role { get; set; }
        }

        public class UpdateSelfViewModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class SendAlreadyBorrowedMessageViewModel
        {
            [Required]
            public int TeacherId { get; set; }
            [Required]
            public int BorrowedTeacherId { get; set; }
            [Required]
            public int ItemId { get; set; }
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

        public async Task<IActionResult> Index()
        {
            var teachers = new List<dynamic>();

            foreach (var t in _context.Teachers.ToList())
            {
                var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == t.Id);

                if (user == null)
                    teachers.Add(new
                    {
                        id = t.Id,
                        name = t.Name,
                        surname = t.Surname,
                        barcode = t.BarCode,
                        borrowedItems = _context.Items.Include(i => i.Events).ThenInclude(e => e.Teacher).ToList()
                            .Count(i => _itemManager.GetWhoBorrowed(i)?.Id == t.Id),
                    });
                else
                    teachers.Add(new
                    {
                        id = t.Id,
                        name = t.Name,
                        surname = t.Surname,
                        barcode = t.BarCode,
                        borrowedItems = _context.Items.Include(i => i.Events).ThenInclude(e => e.Teacher).ToList()
                            .Count(i => _itemManager.GetWhoBorrowed(i)?.Id == t.Id),
                        role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()?.ToLower() ?? "user",
                    });
            }

            return Json(teachers);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Taki nauczyciel nie istnieje.");

            var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == teacher.Id);

            if (user == null)
                return Json(new
                {
                    id = teacher.Id,
                    name = teacher.Name,
                    surname = teacher.Surname,
                    barcode = teacher.BarCode,
                    enableAccount = false,
                });

            return Json(new
            {
                id = teacher.Id,
                name = teacher.Name,
                surname = teacher.Surname,
                barcode = teacher.BarCode,
                enableAccount = true,
                username = user.UserName,
                email = user.Email,
                role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()?.ToLower() ?? "user",
            });
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Add([FromBody] NewTeacherModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Teachers.Any(t =>
                t.Name.ToLower() == model.Name.ToLower() && t.Surname.ToLower() == model.Surname.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim imieniem i nazwiskiem.");

            if (_context.Teachers.Any(t => t.BarCode.ToLower() == model.BarCode.ToLower()))
                return BadRequest("Istnieje już nauczyciel z takim kodem kreskowym.");

            if (_context.Items.Any(i => i.ShortId.ToLower() == model.BarCode.ToLower()))
                return BadRequest("Istnieje już przedmiot z takim kodem kreskowym.");

            var teacher = new Teacher()
            {
                Name = model.Name,
                Surname = model.Surname,
                BarCode = model.BarCode,
            };

            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            //_messageService.SendMessage(teacher,
            //    "Witaj w systemie ewidencji inwentarzu!",
            //    "Wejdź do zakładki pomoc by dowiedzieć się jak skorzystać z systemu.<br>Nie zapomnij też zmienić hasła w zakładce <i>Moje konto</i>.");

            return Json(new
            {
                teacherId = teacher.Id,
            });
        }

        [HttpPost("[action]/{id}")]
        [Authorize(Roles = "Administrator")]
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

            if (_context.Items.Any(i => i.ShortId.ToLower() == model.BarCode.ToLower()))
                return BadRequest("Istnieje już przedmiot z takim kodem kreskowym.");

            bool accountCreated = false;
            string generatedPassword = null;

            if (model.EnableAccount)
            {
                if (String.IsNullOrEmpty(model.Username))
                    return BadRequest("Nie podano nazwy użytkownika.");

                if (model.Username.Length < 2)
                    return BadRequest("Nazwa użytkownika musi mieć przynajmniej 2 znaki.");

                if (model.Role != "administrator" && model.Role != "moderator" && model.Role != "user")
                    return BadRequest("Rola nie jest prawidłowa.");

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

                    _messageService.SendMessage(teacher, "AccountEnabled", user.UserName, generatedPassword, $"{Request.Scheme}://{Request.Host}/");

                    accountCreated = true;
                }
                // Update user account
                else
                {
                    user.UserName = model.Username.Trim();
                    user.Email = model.Email.Trim();
                }

                var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault()?.ToLower() ?? "user";

                if (currentRole != model.Role)
                {
                    var currentUser = _userGetter.GetCurrentUser();

                    if (currentUser.Id == user.Id)
                        return BadRequest("Nie możesz zmienić swojej roli.");

                    if(currentRole != "user")
                        await _userManager.RemoveFromRoleAsync(user, currentRole);

                    if (model.Role == "administrator")
                        await _userManager.AddToRoleAsync(user, Roles.Administrator);
                    else if (model.Role == "moderator")
                        await _userManager.AddToRoleAsync(user, Roles.Moderator);
                }
            }
            else
            {
                var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == teacher.Id);

                var currentUser = _userGetter.GetCurrentUser();

                if (currentUser.Id == user?.Id)
                    return BadRequest("Nie możesz zablokować logowania na własnym koncie.");

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
                _messageService.SendMessage(teacher, "NewBarcode", model.BarCode?.Trim().ToUpper(), $"{Request.Scheme}://{Request.Host}/api/BarCode/Generate?text={model.BarCode.Trim().ToUpper()}");
            }

            _context.SaveChanges();

            if (accountCreated)
                return Json(new
                {
                    generatedPassword = generatedPassword,
                });

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateSelf([FromBody] UpdateSelfViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var user = _userGetter.GetCurrentUser();
            var teacher = user.Teacher;

            if (model.Email != user.Email)
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    model.Email = model.Email.Trim();

                    try
                    {
                        var parsed = new MailAddress(model.Email);
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Podano nieprawidłowy adres E-Mail.");
                    }
                }

                user.Email = model.Email;
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.Password.Length < 6)
                    return BadRequest("Minimalna długość hasła to 6 znaków.");

                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, model.Password);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("[action]/{id}")]
        [Authorize(Roles = Roles.Administrator)]
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
        [Authorize(Roles = Roles.Administrator)]
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

            _messageService.SendMessage(teacher, "PasswordReset", generatedPassword);

            return Json(new
            {
                generatedPassword = generatedPassword,
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendAlreadyBorrowedMessage([FromBody] SendAlreadyBorrowedMessageViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == model.TeacherId);
            if (teacher == null)
                return BadRequest("Nie ma takiego nauczyciela.");

            var borrowedTeacher = _context.Teachers.FirstOrDefault(t => t.Id == model.BorrowedTeacherId);
            if (borrowedTeacher == null)
                return BadRequest("Nie ma takiego nauczyciela.");

            var item = _context.Items.Include(i => i.Template).Include(i => i.Location).FirstOrDefault(i => i.Id == model.ItemId);
            if (item == null)
                return BadRequest("Nie ma takiego przedmiotu");

            //var message = $"Nastąpiła próba pobrania przedmiotu, który nie został zwrócony.<br><br>Identyfikator: {item.ShortId}<br>Nazwa przedmiotu: {item.Name}<br>Położenie: {item.Location?.Name ?? "Brak"}" +
            //    $"<br>Nauczyciel który nie zwrócił przedmiotu: {borrowedTeacher.Name} {borrowedTeacher.Surname}<br>Nauczyciel który próbował zwrócić przedmiot: {teacher.Name} {teacher.Surname}";

            var users = (await _userManager.GetUsersInRoleAsync(Roles.Administrator)).Concat(
                await _userManager.GetUsersInRoleAsync(Roles.Moderator)).ToList();

            var teachers = _context.Users.Include(u => u.Teacher).Where(u => users.Any(u2 => u2.Id == u.Id)).Select(u => u.Teacher).ToList();

            //_messageService.SendMessage(teachers, "Nastąpiła próba pobrania przedmiotu, który nie został zwrócony", message);

            _messageService.SendMessage(teachers, "UnreturnedBorrow", item.ShortId, item.Name,
                item.Location?.Name ?? "Brak", $"{borrowedTeacher.Name} {borrowedTeacher.Surname}",
                $"{teacher.Name} {teacher.Surname}");

            return Ok();
        }
    }
}

