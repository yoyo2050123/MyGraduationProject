using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearnSystem.Areas.Members.Controllers
{
    [Area("Members")]
    [Authorize(Roles = "一般會員,付費會員")]
    public class MemberCenterController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public MemberCenterController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LearanReport()
        {
            return View();


        }
        [HttpGet]
        [Authorize(Roles = "一般會員,付費會員")]
        public async Task<IActionResult> LearnReport()
        {
            string memberId = User.FindFirst("MemberID")?.Value ?? string.Empty;

            var records = await _context.LearnRecordTable
                                        .Where(r => r.MemberID == memberId)
                                        .OrderByDescending(r => r.AnswerTime) // 最新在前
                                        .Take(10)
                                        .ToListAsync();

            return View(records);
        }



        public IActionResult Subscription()
        {
            return View();
        }

        public IActionResult Note()
        {
            return View();
        }
    }
}
