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
            _dbContext.ItemTemplates.Add(template1);

            #endregion

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
                _dbContext.Items.Add(new Item()
                {
                    ShortId = null,
                    Name = rnd.Next(0, 2) == 0 ? "Komputer" : "Mysz komputerowa",
                    Notes = "",
                    Location = (rnd.Next(0, 2) == 0 ? location1 : location2),
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
