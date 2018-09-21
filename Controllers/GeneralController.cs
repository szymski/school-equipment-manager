using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Text.Encodings.Web.Utf8;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SchoolEquipmentManager.Logic;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class GeneralController : Controller
    {
        public class UserLoginViewModel
        {
            [Required(ErrorMessage = "Nie wpisano loginu.")]
            public string Username { get; set; }
            [Required(ErrorMessage = "Nie wpisano hasła.")]
            public string Password { get; set; }
        }

        public class RequestPasswordResetViewModel
        {
            [Required(ErrorMessage = "Nie podano adresu E-Mail.")]
            public string Email { get; set; }
        }

        private AppContext _context;
        private ItemManager _itemManager;
        private UserGetter _userGetter;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;
        private IHostingEnvironment _hostingEnv;

        public GeneralController(AppContext dbContext, ItemManager itemManager, UserGetter userGetter, UserManager<ApplicationUser> userManager, IEmailService emailService, IHostingEnvironment env)
        {
            _context = dbContext;
            _itemManager = itemManager;
            _userGetter = userGetter;
            _userManager = userManager;
            _emailService = emailService;
            _hostingEnv = env;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = _userGetter.GetCurrentUser(u => u.Include(u2 => u2.Teacher).ThenInclude(t => t.Messages));

#if DEBUG
            var devVersion = _hostingEnv.IsDevelopment();
#else
            var devVersion = false;
#endif

            if (user == null)
                return Json(new
                {
                    loggedIn = false,
                    devVersion = devVersion,
                });

            return Json(new
            {
                loggedIn = true,
                devVersion = devVersion,
                teacherId = user.Teacher.Id,
                username = $"{user.Teacher.Name} {user.Teacher.Surname}",
                messageCount = user.Teacher.Messages.Count(m => !m.Read),
                role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()?.ToLower() ?? "user",
            });
        }

        [HttpGet("[action]")]
        public IActionResult GeneralInfo()
        {
            var currentUser = _userGetter.GetCurrentUser();
            var currentTeacher = currentUser.Teacher;

            var borrowedTodayCount = _context.Items.Include(i => i.Events)
                .SelectMany(i => i.Events).Count(e => e.Type == "borrow" && e.Date > DateTime.Today);

            var lastEventsDay = DateTime.Today - TimeSpan.FromDays(7);
            var borrowsData = _context.Items.Include(i => i.Events).SelectMany(i => i.Events).OrderBy(e => e.Date).Where(e => e.Type == "borrow" && e.Date >= lastEventsDay).GroupBy(e => e.Date.Date).Select(g => new
            {
                Date = g.Key,
                BorrowCount = g.Count(),
            }).ToList();

            // Fill gaps in the dates
            for(DateTime date = DateTime.Today; date >= lastEventsDay; date -= TimeSpan.FromDays(1))
                if(borrowsData.All(b => b.Date != date))
                    borrowsData.Add(new
                    {
                        Date = date,
                        BorrowCount = 0,
                    });

            return Json(new
            {
                totalItems = _context.Items.Count(),
                borrowedItems = _context.Items.Include(i => i.Events).ToList().Count(i => !_itemManager.HasBeenReturned(i)),
                borrowedItemsSelf = _context.Items.Include(i => i.Events).ToList().Count(i => _itemManager.GetWhoBorrowed(i) == currentTeacher),
                typesData = _context.Items.Include(i => i.Template).GroupBy(i => i.Template).Select(g => new
                {
                    template = g.Key != null ? g.Key.Id : 0,
                    templateName = g.Key != null ? g.Key.Name : "Brak typu",
                    itemCount = g.Count(),
                }),
                borrowsData = borrowsData.OrderBy(d => d.Date).Select(d => new
                {
                    date = d.Date.ToString("d"),
                    borrowCount = d.BorrowCount,
                }),
            });
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
                return BadRequest("Login lub hasło nieprawidłowe.");

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthHelper.Secret));

                var claims = new List<Claim>
                {
                    new Claim("id", user.Id),
                };

                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (role != null)
                    claims.Add(new Claim(ClaimTypes.Role, role));

                var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "localhost",
                    audience: "localhost",
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

                var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    auth_token = tokenStr,
                });
            }

            return BadRequest("Login lub hasło nieprawidłowe.");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Email = model.Email.Trim();

            try
            {
                var address = new MailAddress(model.Email);
            }
            catch (Exception e)
            {
                return BadRequest("Podany adres E-Mail jest nieprawidłowy.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
            if (user == null)
                return Ok();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"{Request.Scheme}://{Request.Host}/api/General/ResetPassword?email={WebUtility.UrlEncode(user.Email)}&token={WebUtility.UrlEncode(resetToken)}";

            try
            {
                _emailService.SendEmail(user, "Resetowanie hasła", $"Odwiedź następujący link by zresetować hasło: {resetLink}");
            }
            catch (Exception e)
            {
                return BadRequest("Spróbuj ponownie za chwilę lub skontaktuj się z administratorem.");
            }

            return Ok();
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Nie podano adresu e-mail.");

            if (string.IsNullOrEmpty(token))
                return BadRequest("Nie podano tokenu.");

            var user = _context.Users.FirstOrDefault(u => u.Email.Trim().ToLower() == email.Trim().ToLower());
            if (user == null)
                return BadRequest("Nie udało się zresetować hasła.");

            var generatedPassword = new Random().Next(10000000, 99999999).ToString();

            var result = await _userManager.ResetPasswordAsync(user, token, generatedPassword);

            if (!result.Succeeded)
                return BadRequest("Nie udało się zresetować hasła.");

            _emailService.SendEmail(user, "Nowe hasło", $"Nowe hasło: <b>{generatedPassword}</b><br>Twój login: {user.UserName}<br>Tutaj możesz się zalogować: {Request.Scheme}://{Request.Host}");

            return Ok("Udało się zresetować hasło! Na twój adres E-Mail przyjdzie wiadomość z nowym hasłem.");
        }
    }
}

