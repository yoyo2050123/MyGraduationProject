using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Models;
using System.Collections.Generic;

public class QuestionGenerate
{
    private readonly dbJapaneseLearnSystemContext _context;
    private readonly Random _random = new Random();

    public QuestionGenerate(dbJapaneseLearnSystemContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 批次生成指定數量的題目
    /// </summary>
    /// <param name="count">希望生成的題目數量</param>
    public async Task<(int GeneratedCount, List<string> Messages)> GenerateMultipleQuestionsAsync(int count)
    {
        var messages = new List<string>();
        int generated = 0;

        var templates = await _context.QuestionTemplate.ToListAsync();
        if (!templates.Any())
        {
            messages.Add("沒有題目模板可用");
            return (0, messages);
        }

        var words = await _context.Word.ToListAsync();
        if (!words.Any())
        {
            messages.Add("沒有單字可用");
            return (0, messages);
        }

        var triedCombinations = new HashSet<string>(); // 防止無限循環

        while (generated < count)
        {
            // 隨機選模板 + 單字
            var template = templates[_random.Next(templates.Count)];
            var word = words[_random.Next(words.Count)];

            // 生成題目內容
            string content = template.QuestionTemplateText.Replace("{0}", word.Vocabulary);

            // 防止同一輪嘗試重複
            string key = $"{template.QuestionTemplateID}-{word.WordID}";
            if (triedCombinations.Contains(key))
            {
                if (triedCombinations.Count >= templates.Count * words.Count)
                {
                    messages.Add("所有可能的題目組合已用完");
                    break;
                }
                continue;
            }
            triedCombinations.Add(key);

            // 檢查是否已存在
            bool exists = await _context.QuestionInstance.AnyAsync(q => q.QuestionContent == content);
            if (exists) continue;

            // 建立 QuestionInstance
            var questionInstance = new QuestionInstance
            {
                QuestionInstanceID = await GenerateQuestionInstanceIdAsync(),
                QuestionTemplateID = template.QuestionTemplateID,
                WordID = word.WordID,
                QuestionContent = content,
                CreateDate = DateTime.Now
            };

            // 生成選項清單（全新物件，避免追蹤衝突）
            var options = new List<QuestionOption>();

            // 正確答案 OptionID = "1"
            options.Add(new QuestionOption
            {
                QuestionInstanceID = questionInstance.QuestionInstanceID,
                OptionID = "1",
                OptionContent = word.Vocabulary
            });

            // 幹擾選項 OptionID = "2", "3", "4"
            var otherWords = words.Where(w => w.WordID != word.WordID)
                                  .OrderBy(x => _random.Next())
                                  .Take(3)
                                  .ToList();

            int idx = 2;
            foreach (var w in otherWords)
            {
                options.Add(new QuestionOption
                {
                    QuestionInstanceID = questionInstance.QuestionInstanceID,
                    OptionID = idx.ToString(),
                    OptionContent = w.Vocabulary
                });
                idx++;
            }

            // 設定正確答案 OptionID
            questionInstance.AnswerOptionID = "1";

            // 打亂選項順序
            questionInstance.QuestionOption = options.OrderBy(x => _random.Next()).ToList();

            // 將整個 QuestionInstance 連同選項一起加入 DbContext
            _context.QuestionInstance.Add(questionInstance);

            await _context.SaveChangesAsync();

            generated++;
            messages.Add($"生成題目：{content}");
        }

        return (generated, messages);
    }

    /// <summary>
    /// 生成 QuestionInstanceID 流水號，例如 Q0001、Q0002
    /// </summary>
    private async Task<string> GenerateQuestionInstanceIdAsync()
    {
        var lastId = await _context.QuestionInstance
                                   .OrderByDescending(q => q.QuestionInstanceID)
                                   .Select(q => q.QuestionInstanceID)
                                   .FirstOrDefaultAsync();

        int number = 1;
        if (!string.IsNullOrEmpty(lastId) && lastId.Length > 1)
        {
            if (int.TryParse(lastId.Substring(1), out int n))
            {
                number = n + 1;
            }
        }

        return $"Q{number:D4}";
    }
}
