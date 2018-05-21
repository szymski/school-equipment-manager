using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager
{
    public class DbInitializer
    {
        private AppContext _dbContext;

        public DbInitializer(AppContext dbContext)
        {
            _dbContext = dbContext;
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
                Name = "Mysz komputerowa",
                Description = "Tania, łatwo się psuje"
            };
            _dbContext.ItemTemplates.Add(template2);

            var template3 = new ItemTemplate()
            {
                Name = "Laptop",
                Description = "Raz działa, a raz nie"
            };
            _dbContext.ItemTemplates.Add(template3);

            #endregion

            _dbContext.Teachers.Add(new Teacher()
            {
                Name = "Bartek",
                Surname = "Kurpinix",
                BarCode = "2137",
            });

            _dbContext.Teachers.Add(new Teacher()
            {
                Name = "Szymon",
                Surname = "Dzankowski",
                BarCode = "1337",
            });

            _dbContext.Teachers.Add(new Teacher()
            {
                Name = "Richard",
                Surname = "Hendricks",
                BarCode = "PiedPiper",
            });

            var teacherOne = new Teacher()
            {
                Name = "Bertram",
                Surname = "Gilfoyle",
                BarCode = "BitcoinPrice",
            };

            _dbContext.Teachers.Add(teacherOne);

            _dbContext.Items.Add(new Item()
            {
                ShortId = "S8-MY-0001",
                Location = location1,
                Template = template2,
            });

            _dbContext.Items.Add(new Item()
            {
                ShortId = null,
                Notes = "Nie działa",
                Location = location2,
                Template = template1,
            });

            _dbContext.Items.Add(new Item()
            {
                ShortId = "S10-KO-0001",
                Notes = "Nie działa",
                Location = location2,
                Template = template1,
            });

            var rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                var events = new List<BorrowEvent>()
                {
                    new BorrowEvent()
                    {
                        Teacher = teacherOne,
                        Date = DateTime.Now - TimeSpan.FromMinutes(45),
                        Type = "borrow",
                    }
                };

                if(rnd.Next(0, 2) == 0)
                    events.Add(new BorrowEvent()
                    {
                        Teacher = teacherOne,
                        Date = DateTime.Now,
                        Type = "return",
                    });

                var item = new Item()
                {
                    ShortId = null,
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
