using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public class MessageService : IMessageService
    {
        private AppContext _context;
        private IConfiguration _configuration;
        private IEmailService _emailService;

        public MessageService(AppContext dbContext, IConfiguration configuration, IEmailService emailService)
        {
            _context = dbContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        public Task SendMessage(Teacher recipent, string templateName, params object[] bodyFormatValues)
        {
            var titleFormat = _configuration["Messages:TitleFormat"];
            var bodyFormat = _configuration["Messages:BodyFormat"];

            var innerTitle = _configuration[$"Messages:Templates:{templateName}:Title"];
            var innerBody = _configuration[$"Messages:Templates:{templateName}:Body"];

            var finalTitle = string.Format(titleFormat, innerTitle);
            var finalBody = string.Format(bodyFormat, recipent.Name, string.Format(innerBody, bodyFormatValues));

            _context.Messages.Add(new Message()
            {
                Recipent = recipent,
                Date = DateTime.Now,
                Title = finalTitle,
                Body = finalBody,
                Read = false,
            });

            _context.SaveChanges();

            var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == recipent.Id);

            return Task.Run(() =>
            {
                if (user != null)
                    try
                    {
                        _emailService.SendEmail(user, finalTitle, finalBody);
                    }
                    catch (Exception e)
                    {

                    }
            });
        }

        public Task SendMessage(IEnumerable<Teacher> recipents, string templateName, params object[] bodyFormatValues)
        {
            var users = _context.Users.Include(u => u.Teacher).Where(u => recipents.Any(r => r.Id == u.Teacher.Id)).ToList();

            return Task.Run(() =>
            {
                foreach (var teacher in recipents)
                    SendMessage(teacher, templateName, bodyFormatValues);

            });
        }

        public Task SendMessageToAll(string templateName, params object[] bodyFormatValues)
        {
            var users = _context.Users.Include(u => u.Teacher).ToList();

            return Task.Run(() =>
            {
                foreach (var user in users)
                    SendMessage(user.Teacher, templateName, bodyFormatValues).Wait();
            });
        }

        //public Task SendMessage(Teacher recipent, string title, string body)
        //{
        //    _context.Messages.Add(new Message()
        //    {
        //        Recipent = recipent,
        //        Date = DateTime.Now,
        //        Title = title,
        //        Body = body,
        //        Read = false,
        //    });

        //    _context.SaveChanges();

        //    var user = _context.Users.Include(u => u.Teacher).FirstOrDefault(u => u.Teacher.Id == recipent.Id);

        //    return Task.Run(() =>
        //    {
        //        if (user != null)
        //            try
        //            {
        //                _emailService.SendEmail(user, title, body);
        //            }
        //            catch (Exception e)
        //            {

        //            }
        //    });
        //}

        //public Task SendMessage(IEnumerable<Teacher> recipents, string title, string body)
        //{
        //    foreach (var teacher in recipents)
        //    {
        //        _context.Messages.Add(new Message()
        //        {
        //            Recipent = teacher,
        //            Date = DateTime.Now,
        //            Title = title,
        //            Body = body,
        //            Read = false,
        //        });
        //    }

        //    _context.SaveChanges();

        //    var users = _context.Users.Include(u => u.Teacher).Where(u => recipents.Any(r => r.Id == u.Teacher.Id)).ToList();

        //    return Task.Run(() =>
        //    {
        //        try
        //        {
        //            _emailService.SendEmail(users, title, body);
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //    });
        //}

        //public Task SendMessageToAll(string title, string body)
        //{
        //    foreach (var teacher in _context.Teachers)
        //        _context.Messages.Add(new Message()
        //        {
        //            Recipent = teacher,
        //            Date = DateTime.Now,
        //            Title = title,
        //            Body = body,
        //            Read = false,
        //        });

        //    _context.SaveChanges();

        //    var users = _context.Users.ToList();

        //    return Task.Run(() =>
        //    {
        //        foreach (var user in users)
        //            try
        //            {
        //                _emailService.SendEmail(user, title, body);
        //            }
        //            catch (Exception e)
        //            {

        //            }
        //    });
        //}
    }
}
