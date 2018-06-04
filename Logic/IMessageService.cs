using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public interface IMessageService
    {
        Task SendMessage(Teacher recipent, string title, string body);
        Task SendMessage(IEnumerable<Teacher> recipents, string title, string body);
        Task SendMessageToAll(string title, string body);
    }
}
