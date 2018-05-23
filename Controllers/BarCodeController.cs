using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Logic;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class BarCodeController : Controller
    {
        private AppContext _context;
        private BarCodeManager _barcodeManager;
        private ItemManager _itemManager;

        public BarCodeController(AppContext dbContext, BarCodeManager barcodeManager, ItemManager itemManager)
        {
            _context = dbContext;
            _barcodeManager = barcodeManager;
            _itemManager = itemManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Generate(string text)
        {
            if (text == null)
                return Content("Please specify barcode text.");

            var cl = new HttpClient();
            byte[] data = await cl.GetByteArrayAsync($"http://bwipjs-api.metafloor.com/?bcid=code128&text={text}&scale=3&includetext");

            return File(data, "image/png");
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetBarcodesForTeacher(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (teacher == null)
                return BadRequest("Nie ma nauczyciela o takim identyfikatorze.");

            return Json(new
            {
                @base = teacher.BarCode,
                borrow = _barcodeManager.GetBorrowCode(teacher),
                @return = _barcodeManager.GetReturnCode(teacher),
            });
        }

        [HttpGet("[action]")]
        public IActionResult ParseCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Nie wprowadzono kodu.");

            code = code.Trim().ToLower();

            if(code == "pobr")
                return Json(new
                {
                    type = "borrow",
                });

            if (code == "zwrot")
                return Json(new
                {
                    type = "return",
                });

            var teacher = _context.Teachers.FirstOrDefault(t => t.BarCode.ToLower() == code);
            if (teacher != null)
                return Json(new
                {
                    type = "teacher",
                    id = teacher.Id,
                });

            var item = _context.Items.Include(i => i.Events).ThenInclude(e => e.Teacher).FirstOrDefault(i => i.ShortId.ToLower() == code);
            if (item != null)
            {
                var whoBorrowed = _itemManager.GetWhoBorrowed(item);

                return Json(new
                {
                    type = "item",
                    id = item.Id,
                    alreadyBorrowed = whoBorrowed != null,
                    whoBorrowed = whoBorrowed?.Id,
                });
            }

            return BadRequest("Kod nie został rozpoznany.");
        }
    }
}
