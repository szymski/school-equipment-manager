using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Logic;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class GeneralController : Controller
    {
        private AppContext _context;
        private ItemManager _itemManager;

        public GeneralController(AppContext dbContext, ItemManager itemManager)
        {
            _context = dbContext;
            _itemManager = itemManager;
        }

        [HttpGet("[action]")]
        public IActionResult GeneralInfo()
        {
            return Json(new
            {
                totalItems = _context.Items.Count(),
                borrowedItems = _context.Items.Include(i => i.Events).ToList().Count(i => !_itemManager.HasBeenReturned(i)),
            });
        }
    }
}

