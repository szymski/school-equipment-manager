using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public interface IEmailService
    {
        void SendEmail(ApplicationUser recipent, string title, string body);
        void SendEmail(string address, string title, string body);
    }
}
