using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using SchoolEquipmentManager.Logic;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Controllers
{
    [Route("api/[controller]")]
    public class BarCodeController : Controller
    {
        private AppContext _context;
        private BarCodeManager _barcodeManager;

        public BarCodeController(AppContext dbContext, BarCodeManager barcodeManager)
        {
            _context = dbContext;
            _barcodeManager = barcodeManager;
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
    }
}
