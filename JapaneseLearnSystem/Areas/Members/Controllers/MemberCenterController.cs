using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearnSystem.Areas.Members.Controllers
{
    [Area("Members")]
    [Authorize(Roles = "一般會員,付費會員")]
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

        public IActionResult PracticePrepare()
        {
            return View();
        }


    }
}
