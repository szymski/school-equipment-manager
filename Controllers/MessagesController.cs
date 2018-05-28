using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MessagesController : Controller
    {
        private AppContext _context;
        private UserGetter _userGetter;

        public MessagesController(AppContext dbContext, UserGetter userGetter)
        {
            _context = dbContext;
            _userGetter = userGetter;
        }

        public IEnumerable<dynamic> Index()
        {
            var user = _userGetter.GetCurrentUser(u => u.Include(t => t.Teacher.Messages));
            return user.Teacher.Messages.OrderByDescending(m => m.Date).Select(m => new
            {
                id = m.Id,
                date = m.Date.ToString("G"),
                title = m.Title,
                read = m.Read,
            });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Get(int id)
        {
            var message = _context.Messages.Include(m => m.Recipent).FirstOrDefault(m => m.Id == id);
            if (message == null)
                return BadRequest("Nie ma wiadomości o takim id.");

            if (message.Recipent.Id != _userGetter.GetCurrentUser().Teacher.Id)
                return BadRequest("Nie masz dostępu do tej wiadomości.");

            message.Read = true;
            _context.SaveChanges();

            return Json(new
            {
                id = message.Id,
                date = message.Date.ToString("G"),
                title = message.Title,
                body = message.Body,
                read = message.Read,
            });
        }
    }
}
