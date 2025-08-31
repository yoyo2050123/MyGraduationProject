using System.Diagnostics;
using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearnSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


//�qgithub��U�Ӫ��{���X �O�o�ק�dbJapaneseLearnSystemContext.cs��Program.cs���s�u�r��
//�N�O"Data Source=KAZE;Database=dbJapaneseLearnSystem;TrustServerCertificate=True;User ID=abc;Password=123"�o��