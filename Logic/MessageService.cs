using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public class MessageService : IMessageService
    {
        private AppContext _context;
        private IEmailService _emailService;

        public MessageService(AppContext dbContext, IEmailService emailService)
        {
            _context = dbContext;
            _emailService = emailService;
        }

        public Task SendMessage(Teacher recipent, string title, string body)
        {
            _context.Messages.Add(new Message()
            {
                Recipent = recipent,
                Date = DateTime.Now,
                Title = title,
                Body = body,
                Read = false,
            });

            _context.SaveChanges();

            var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == recipent.Id);

            return Task.Run(() =>
            {
                if (user != null)
                    try
                    {
                        _emailService.SendEmail(user, title, body);
                    }
                    catch (Exception e)
                    {

                    }
            });
        }

        public Task SendMessage(IEnumerable<Teacher> recipents, string title, string body)
        {
            foreach (var teacher in recipents)
            {
                _context.Messages.Add(new Message()
                {
                    Recipent = teacher,
                    Date = DateTime.Now,
                    Title = title,
                    Body = body,
                    Read = false,
                });
            }

            _context.SaveChanges();

            var users = _context.Users.Include(u => u.Teacher).Where(u => recipents.Any(r => r.Id == u.Teacher.Id)).ToList();

            return Task.Run(() =>
            {
                try
                {
                    _emailService.SendEmail(users, title, body);
                }
                catch (Exception e)
                {

                }
            });
        }

        public Task SendMessageToAll(string title, string body)
        {
            foreach (var teacher in _context.Teachers)
                _context.Messages.Add(new Message()
                {
                    Recipent = teacher,
                    Date = DateTime.Now,
                    Title = title,
                    Body = body,
                    Read = false,
                });

            _context.SaveChanges();

            var users = _context.Users.ToList();

            return Task.Run(() =>
            {
                foreach (var user in users)
                    try
                    {
                        _emailService.SendEmail(user, title, body);
                    }
                    catch (Exception e)
                    {
                        
                    }
            });
        }
    }
}
