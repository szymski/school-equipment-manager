using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SchoolEquipmentManager.Controllers
{
    public class BarCodeController : Controller
    {
        public async Task<IActionResult> Generate(string text)
        {
            if (text == null)
                return Content("Please specify barcode text.");

            var cl = new HttpClient();
            byte[] data = await cl.GetByteArrayAsync($"http://bwipjs-api.metafloor.com/?bcid=code128&text={text}&scale=3&includetext");

            return File(data, "image/png");
        }
    }
}
