using JapaneseLearnSystem.Areas.Members.Models;
using JapaneseLearnSystem.Models;
using JapaneseLearnSystem.Services;
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
        private readonly QuestionResultService _resultService;
        private readonly LearnReportService _reportService;

        public QuestionController(dbJapaneseLearnSystemContext context, QuestionResultService resultService,LearnReportService learnReportService)
        {
            _context = context;
            _resultService = resultService;
            _reportService = learnReportService;

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
            TempData["ResultMessage"] = null; // 清掉上一次結果
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
        public async Task<IActionResult> QuestionPage(List<QuestionPage> submittedAnswers)
        {
            if (submittedAnswers == null || submittedAnswers.Count == 0)
            {
                TempData["ResultMessage"] = "沒有收到任何答案。";
                return RedirectToAction("QuestionResult");
            }

            // 取得會員 ID（假設是 string 型別）
            string memberId = User.FindFirst("MemberID")?.Value ?? string.Empty;

            // 呼叫 Service 生成完整答題結果
            var results = await _resultService.GenerateResultsAsync(submittedAnswers);

            // 計算正確題數
            int correctCount = results.Count(r => r.IsCorrect);

            int learnedWordCount = results
                .Select(r => r.WordID)  // 取每題的 WordID
                .Distinct()             // 去掉重複
                .Count();               // 算不同單字數量

            // 計算總題數
            int totalAnswers = results.Count;

            // 產生學習報告並存入資料庫
            if (!string.IsNullOrEmpty(memberId))
            {
                await _reportService.CreateLearnReportAsync(
                    memberId,
                    totalAnswers,
                    correctCount,
                    learnedWordCount
                );
            }

            // 給使用者訊息
            TempData["ResultMessage"] = $"你答對 {correctCount} / {totalAnswers} 題！";

            // 將結果傳給 QuestionResult View
            return View("QuestionResult", results);
        }




        public IActionResult QuestionResult()
        {
            return View();
        }


    }
}
