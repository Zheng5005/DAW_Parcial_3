using DAW_Parcial_3.Tests;
using Microsoft.AspNetCore.Mvc;

namespace DAW_Parcial_3.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult RunSeleniumTest()
        //{
        //    var seleniumTest = new SeleniumTests();
        //    seleniumTest.RunTest();

        //    return Content("Selenium Test Completed");
        //}


        public IActionResult RunSeleniumTest()
        {
            var seleniumTest = new SeleniumTests();
            List<string> searchResults = seleniumTest.RunTest();

            return View(searchResults);
        }
    }
}
