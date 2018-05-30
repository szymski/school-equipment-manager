using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public interface IMessageService
    {
        void SendMessage(Teacher recipent, string title, string body);
        void SendMessageToAll(string title, string body);
    }
}
