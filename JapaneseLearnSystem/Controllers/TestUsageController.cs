using Microsoft.AspNetCore.Mvc;
using JapaneseLearnSystem.Services;

namespace JapaneseLearnSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestUsageController : ControllerBase
    {
        private readonly MemberUsageService _usageService;

        public TestUsageController(MemberUsageService usageService)
        {
            _usageService = usageService;
        }

        // 測試塞爆單字
        [HttpPost("word")]
        public IActionResult UseWords(string memberId, int count)
        {
            try
            {
                _usageService.Use(memberId, wordsUsed: count);
                return Ok($"成功使用 {count} 個單字");
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤: {ex.Message}");
            }
        }

        // 測試塞爆筆記
        [HttpPost("note")]
        public IActionResult UseNotes(string memberId, int count)
        {
            try
            {
                _usageService.Use(memberId, notesUsed: count);
                return Ok($"成功使用 {count} 筆筆記");
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤: {ex.Message}");
            }
        }

        // 測試塞爆題目
        [HttpPost("question")]
        public IActionResult UseQuestions(string memberId, int count)
        {
            try
            {
                _usageService.Use(memberId, questionsUsed: count);
                return Ok($"成功使用 {count} 題");
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤: {ex.Message}");
            }
        }
    }
}
