using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public class UserGetter
    {
        private UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor _contextAccessor;

        public UserGetter(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Returns the current user.
        /// Can return null if user not logged in.
        /// </summary>
        public ApplicationUser GetCurrentUser()
        {
            var idClaim = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");

            if (idClaim == null)
                return null;

            var users = _userManager.Users.Include(u => u.Teacher).ToList();

            return users.FirstOrDefault(u => u.Id == idClaim.Value);

            //var userId = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            //return _userManager.Users.FirstOrDefault(u => u.Id == userId);
        }

        public ApplicationUser GetCurrentUser(Func<IQueryable<ApplicationUser>, IQueryable<ApplicationUser>> includeFunc)
        {
            var idClaim = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");

            if (idClaim == null)
                return null;

            var users = includeFunc(_userManager.Users.Include(u => u.Teacher)).ToList();

            return users.FirstOrDefault(u => u.Id == idClaim.Value);
        }
    }
}
