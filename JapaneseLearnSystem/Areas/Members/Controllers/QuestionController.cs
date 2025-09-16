using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JapaneseLearnSystem.Models; 

namespace JapaneseLearnSystem.Areas.Members.Controllers
{
    [Area("Members")]
    [Authorize(Roles = "一般會員,付費會員")]
    public class QuestionController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
            
        }

        
        // GET: Members/Question/PracticePrepare
        [HttpGet]
        public IActionResult PracticePrepare()
        {
            return View();
        }

        // POST: Members/Question/PracticePrepare
        [HttpPost]

        public IActionResult PracticePrepare(int questionCount)
        {
            if (questionCount <= 0)
            {
                ModelState.AddModelError("", "請輸入大於 0 的題目數量。");
                return View();
            }

            // 跳轉到 Practice Action，並傳入題目數量
            return RedirectToAction("QuestionPage", new { count = questionCount });
        }
        

        public IActionResult QuestionPage()
        {
            return View();
        }
        public IActionResult QuestionResult()
        {
            return View();
        }


    }
}
