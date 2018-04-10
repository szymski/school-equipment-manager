using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolEquipmentManager.Logic;

namespace UnitTest
{
    [TestClass]
    public class BarCodeTests
    {
        private BarCodeManager _manager;

        public BarCodeTests()
        {
            _manager = new BarCodeManager();
        }

        [TestMethod]
        public void test_generate_codes()
        {
        }
    }
}
