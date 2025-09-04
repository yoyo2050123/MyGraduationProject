using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearnSystem.Areas.Admin.Controllers
{
    public class Subscriptions : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
