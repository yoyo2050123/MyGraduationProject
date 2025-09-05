using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearnSystem.Areas.Members.Controllers
{
    public class MemberCenterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LearanReport()
        {
            return View();
        }
    }
}
