using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public class MessageService : IMessageService
    {
        private AppContext _context;

        public MessageService(AppContext dbContext)
        {
            _context = dbContext;
        }

        public void SendMessage(Teacher recipent, string title, string body)
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
        }

        public void SendMessageToAll(string title, string body)
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
        }
    }
}
