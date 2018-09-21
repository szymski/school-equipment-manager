using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public interface IMessageService
    {
        Task SendMessage(Teacher recipent, string templateName, params object[] bodyFormatValues);
        Task SendMessage(IEnumerable<Teacher> recipents, string templateName, params object[] bodyFormatValues);
        Task SendMessageToAll(string templateName, params object[] bodyFormatValues);
    }
}
