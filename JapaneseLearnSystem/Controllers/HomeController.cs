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


//從github抓下來的程式碼 記得修改dbJapaneseLearnSystemContext.cs跟Program.cs的連線字串
//就是"Data Source=KAZE;Database=dbJapaneseLearnSystem;TrustServerCertificate=True;User ID=abc;Password=123"這串
//記得以後要修改Program.cs的話 也要同步修改這邊的連線字串，就是"dbJapaneseLearnSystemConnection"，這個要跟appsettings.json的連線字串名稱一樣