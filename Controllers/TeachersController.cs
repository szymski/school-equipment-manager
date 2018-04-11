using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class TeachersController : Controller
    {
        private AppContext _context;

        public TeachersController(AppContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<dynamic> Index()
        {
            return _context.Teachers.Select(t => new
            {
                id = t.Id,
                name = t.Name,
                surname = t.Surname,
                barcode = t.BarCode,
            });
        }
    }
}
