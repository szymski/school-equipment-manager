using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    /// <summary>
    /// Elementy w kodzie kreskowym:
    /// - Skrót rodzaju przedmiotu
    /// - Skrót lokalizacji
    /// - Numer seryjny z TEBu
    /// - Numer identyfikacyjny
    /// </summary>
    public class BarCodeManager
    {
        public string GetBorrowCode(Teacher teacher)
        {
            return $"{teacher.BarCode}.POBR";
        }

        public string GetReturnCode(Teacher teacher)
        {
            return $"{teacher.BarCode}.ZWROT";
        }
    }

    public class BarCodeDefinition
    {
        public ItemTemplate Template { get; set; }
        public Location Location { get; set; }
        public string AdditionalSerialNumber { get; set; }
        public string Identifier { get; set; }
    }

    public class InvalidBarCodeException : Exception
    {
        
    }
}
