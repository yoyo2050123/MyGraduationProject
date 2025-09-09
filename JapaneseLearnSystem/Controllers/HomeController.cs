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

//_ViewStart.cshtml要放在Views資料夾裡面那層，不要放進去Shared資料夾裡面
//資料中介表格的命名規則是兩個資料表名稱中間用底線連接起來，然後兩個資料表名稱都要用複數形式
//資料中介表要Scaffold的話要注意，因為他沒有辦法手動去修改，所以要多建立一個空白的欄位來讓它變成實體，這樣Scaffold才會能夠手動操作這個表格