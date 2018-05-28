using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SchoolEquipmentManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Teacher Teacher { get; set; }
    }
}
