using JapaneseLearnSystem.Areas.Members.Models;
using JapaneseLearnSystem.Models;
using JapaneseLearnSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Area("Members")]
[Authorize(Roles = "一般會員,付費會員")]
public class QuestionController : Controller
{
    private readonly dbJapaneseLearnSystemContext _context;
    private readonly MemberUsageService _usageService;
    private readonly QuestionResultService _resultService;
    private readonly LearnReportService _reportService;

    public QuestionController(
        dbJapaneseLearnSystemContext context,
        QuestionResultService resultService,
        LearnReportService reportService,
        MemberUsageService usageService)
    {
        _context = context;
        _resultService = resultService;
        _reportService = reportService;
        _usageService = usageService;
    }

    [HttpGet]
    public IActionResult PracticePrepare()
    {
        string memberId = User.FindFirst("MemberID")?.Value;
        int? remainingQuestions = _usageService.GetRemainingQuestions(memberId);

        ViewBag.RemainingQuestions = remainingQuestions;

        return View(new PracticePrepare());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PracticePrepare(PracticePrepare model)
    {
        string memberId = User.FindFirst("MemberID")?.Value;
        int? remainingQuestions = _usageService.GetRemainingQuestions(memberId);

        if (!ModelState.IsValid)
        {
            ViewBag.RemainingQuestions = remainingQuestions;
            return View(model);
        }

        // ✅ 檢查至少要有選一個 N1–N5
        if (model.SelectedLevels == null || !model.SelectedLevels.Any())
        {
            ModelState.AddModelError("SelectedLevels", "請至少選擇一個級別 (N1–N5)");
            ViewBag.RemainingQuestions = remainingQuestions;
            return View(model);
        }

        if (remainingQuestions.HasValue && model.Count > remainingQuestions.Value)
        {
            ModelState.AddModelError("Count", $"你今天最多還能玩 {remainingQuestions.Value} 題");
            ViewBag.RemainingQuestions = remainingQuestions;
            return View(model);
        }

        // 存成字串，避免 TempData List 失效
        TempData["SelectedLevels"] = string.Join(",", model.SelectedLevels);
        TempData.Keep("SelectedLevels"); // 保留到下一個 GET

        return RedirectToAction("QuestionPage", new { count = model.Count });
    }


    [HttpGet]
    public async Task<IActionResult> QuestionPage(int count = 1)
    {


        TempData["ResultMessage"] = null;

        TempData.Keep("SelectedLevels");

        var selectedLevels = (TempData["SelectedLevels"] as string)?.Split(',').ToList() ?? new List<string>();

        var selectedJLPTIDs = await _context.JLPTLevel
            .Where(l => selectedLevels.Contains(l.JLPTLevelName))
            .Select(l => l.JLPTLevelID)
            .ToListAsync();

        var questionInstances = await _context.QuestionInstance
            .Include(q => q.Word)
            .Where(q => selectedJLPTIDs.Contains(q.Word.JLPTLevelID))
            .OrderBy(q => Guid.NewGuid())
            .Take(count)
            .ToListAsync();

        var modelList = new List<QuestionPage>();
        foreach (var q in questionInstances)
        {
            var options = await _context.QuestionOption
                .Where(o => o.QuestionInstanceID == q.QuestionInstanceID)
                .ToListAsync();

            modelList.Add(new QuestionPage
            {
                QuestionInstanceID = q.QuestionInstanceID,
                QuestionContent = q.QuestionContent,
                Options = options.OrderBy(o => Guid.NewGuid()).ToList()
            });
        }

        return View(modelList);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuestionResult(List<QuestionPage> model)
    {
        if (model == null || !model.Any())
            return RedirectToAction("PracticePrepare");

        var resultList = new List<QuestionResult>();

        foreach (var q in model)
        {
            // 取得該題正確答案的 Option
            var correctOption = await _context.QuestionOption
                .Where(o => o.QuestionInstanceID == q.QuestionInstanceID && o.OptionID == _context.QuestionInstance
                    .Where(qq => qq.QuestionInstanceID == q.QuestionInstanceID)
                    .Select(qq => qq.AnswerOptionID)
                    .FirstOrDefault())
                .FirstOrDefaultAsync();

            // 取得使用者選的 Option
            var userOption = await _context.QuestionOption
                .Where(o => o.QuestionInstanceID == q.QuestionInstanceID && o.OptionID.ToString() == q.SelectedOptionID)
                .FirstOrDefaultAsync();

            // 取得題目內容
            var questionContent = await _context.QuestionInstance
                .Where(qq => qq.QuestionInstanceID == q.QuestionInstanceID)
                .Select(qq => qq.QuestionContent)
                .FirstOrDefaultAsync();

            resultList.Add(new QuestionResult
            {
                QuestionInstanceID = q.QuestionInstanceID,
                QuestionContent = questionContent ?? "",
                CorrectAnswer = correctOption?.OptionID.ToString() ?? "",
                CorrectAnswerContent = correctOption?.OptionContent ?? "",
                UserAnswer = userOption?.OptionID.ToString() ?? "",
                UserAnswerContent = userOption?.OptionContent ?? "",
                Options = (await _context.QuestionOption
                            .Where(o => o.QuestionInstanceID == q.QuestionInstanceID)
                            .Select(o => o.OptionContent)
                            .ToListAsync()) ?? new List<string>(),
                WordID = await _context.QuestionInstance
                            .Where(qq => qq.QuestionInstanceID == q.QuestionInstanceID)
                            .Select(qq => qq.WordID.ToString())
                            .FirstOrDefaultAsync()
            });
        }

        return View(resultList);
    }



}
