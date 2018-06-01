using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        private AppContext _context;
        private ItemManager _itemManager;
        private UserGetter _userGetter;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public GeneralController(AppContext dbContext, ItemManager itemManager, UserGetter userGetter, UserManager<ApplicationUser> userManager)
        {
            _context = dbContext;
            _itemManager = itemManager;
            _userGetter = userGetter;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult GetUserInfo()
        {
            var user = _userGetter.GetCurrentUser(u => u.Include(u2 => u2.Teacher).ThenInclude(t => t.Messages));

            if (user == null)
                return Json(new
                {
                    loggedIn = false,
                });

            return Json(new
            {
                loggedIn = true,
                teacherId = user.Teacher.Id,
                username = $"{user.Teacher.Name} {user.Teacher.Surname}",
                messageCount = user.Teacher.Messages.Count(m => !m.Read),
            });
        }

        [HttpGet("[action]")]
        public IActionResult GeneralInfo()
        {
            var borrowedTodayCount = _context.Items.Include(i => i.Events)
                .SelectMany(i => i.Events).Count(e => e.Type == "borrow" && e.Date > DateTime.Today);

            return Json(new
            {
                totalItems = _context.Items.Count(),
                borrowedItems = _context.Items.Include(i => i.Events).ToList().Count(i => !_itemManager.HasBeenReturned(i)),
                borrowedTodayCount = borrowedTodayCount,
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

                var claims = new[]
                {
                    new Claim("id", user.Id),
                };

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
    }
}

