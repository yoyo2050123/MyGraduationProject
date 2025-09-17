using JapaneseLearnSystem.Areas.Members.Models;
using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View(new PracticePrepare());
        }

        // POST: Members/Question/PracticePrepare
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PracticePrepare(PracticePrepare model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Count < 1 || model.Count > 20)
            {
                ModelState.AddModelError("Count", "題數必須在 1~20 題");
                return View(model);
            }


           
            // 成功 → Redirect 到 QuestionPage，使用 Query String 傳 count
            return RedirectToAction("QuestionPage", "Question", new { area = "Members", count = model.Count });
        
        }

        [HttpGet]
        public async Task<IActionResult> QuestionPage(int count = 1)
        {
            // 隨機抽題
            var questionInstances = await _context.QuestionInstance
                .OrderBy(q => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            var modelList = new List<QuestionPage>();

            foreach (var q in questionInstances)
            {
                // 取得該題所有選項
                var options = await _context.QuestionOption
                    .Where(o => o.QuestionInstanceID == q.QuestionInstanceID)
                    .AsNoTracking()
                    .ToListAsync();

                // 打亂選項順序
                var shuffledOptions = options.OrderBy(o => Guid.NewGuid()).ToList();

                modelList.Add(new QuestionPage
                {
                    QuestionInstanceID = q.QuestionInstanceID,
                    QuestionContent = q.QuestionContent,
                    Options = shuffledOptions
                });
            }

            return View(modelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QuestionPage(List<QuestionPage> submittedAnswers)
        {
            int correctCount = 0;

            foreach (var answer in submittedAnswers)
            {
                // 找出正確答案
                var question = _context.QuestionInstance
                .FirstOrDefault(q => q.QuestionInstanceID == answer.QuestionInstanceID);

                // 注意：你這裡可以改用 AnswerOptionID 去比對
                if (answer.SelectedOptionID == question?.AnswerOptionID)
                {
                    correctCount++;
                }
            }

            TempData["ResultMessage"] = $"你答對 {correctCount} / {submittedAnswers.Count} 題！";

            return RedirectToAction(nameof(QuestionPage), new { count = submittedAnswers.Count });
        }


        public IActionResult QuestionResult()
        {
            return View();
        }


    }
}
