using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager
{
    public class DbInitializer
    {
        private AppContext _dbContext;
        private UserManager<ApplicationUser> _userManager;

        public DbInitializer(AppContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Items.Any())
                return;

            #region Locations

            var location1 = new Location()
            {
                Name = "Sala 8",
            };
            _dbContext.Locations.Add(location1);

            var location2 = new Location()
            {
                Name = "Sala 10",
            };
            _dbContext.Locations.Add(location2);

            #endregion

            #region Templates

            var template1 = new ItemTemplate()
            {
                Name = "Komputer",
                Description = "Taki tam komputerek"
            };
            _dbContext.ItemTemplates.Add(template1);

            var template2 = new ItemTemplate()
            {
                Name = "Laptop",
                Description = "Raz działa, a raz nie"
            };
            _dbContext.ItemTemplates.Add(template2);

            #endregion

            #region Teachers

            var teacher1 = new Teacher()
            {
                Name = "Karol",
                Surname = "Wojtyła",
                BarCode = "KAR-WOJ-592479",
            };
            _dbContext.Teachers.Add(teacher1);

            var teacher2 = new Teacher()
            {
                Name = "Jarosław",
                Surname = "Polskezbaw",
                BarCode = "JAR-POL-582307",
            };
            _dbContext.Teachers.Add(teacher2);

            var teacher3 = new Teacher()
            {
                Name = "Szymon",
                Surname = "Jankowski",
                BarCode = "SZY-JAN-021370",
            };
            _dbContext.Teachers.Add(teacher3);

            var task = _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "admin",
                Teacher = teacher3,
            }, "123456");
            task.Wait();

            var result = task.Result;
            if(!result.Succeeded)
                throw new Exception("User creation failed.");

            var task2 = _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "karol",
                Teacher = teacher1,
            }, "12345678");
            task2.Wait();

            #endregion

            var rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                var events = new List<BorrowEvent>();

                if (rnd.Next(0, 2) == 0)
                {
                    events.Add(new BorrowEvent()
                    {
                        Teacher = rnd.Next(0, 1) == 0 ? teacher1 : teacher2,
                        Date = DateTime.Now - TimeSpan.FromMinutes(45),
                        Type = "borrow",
                    });
                }

                var item = new Item()
                {
                    ShortId = rnd.Next(0, 2) == 0 ? null : $"{rnd.Next(100000000, 999999999)}.{rnd.Next(10000, 99999)}",
                    Template = rnd.Next(0, 2) == 0 ? template1 : template2,
                    Notes = "",
                    Location = (rnd.Next(0, 2) == 0 ? location1 : location2),
                    Events = events,
                };
                _dbContext.Items.Add(item);
            }

            _dbContext.SaveChanges();
        }
    }
}
