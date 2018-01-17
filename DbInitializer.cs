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

            if(_dbContext.Items.Any())
                return;

            _dbContext.Items.Add(new Item()
            {
                ShortId = 2137,
                Name = "Myszek komputerowy",
                Description = "Przydatny do klikania",
                Location = "Nie wiadomo",
            });

            _dbContext.Items.Add(new Item()
            {
                ShortId = 1337,
                Name = "Monitor",
                Description = "Nie działa",
                Location = "Sala 8",
            });

            var rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                _dbContext.Items.Add(new Item()
                {
                    ShortId = rnd.Next(100, 10000),
                    Name = rnd.Next(0, 2) == 0 ? "Komputer" : "Mysz komputerowa",
                    Description = "",
                    Location = "Sala " + (rnd.Next(0, 2) == 0 ? "8" : "10"),
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
