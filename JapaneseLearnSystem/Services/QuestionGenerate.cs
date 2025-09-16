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
            string content = template.QuestionTemplate1.Replace("{Word}", word.Vocabulary);

            // 防止同一輪嘗試重複
            string key = $"{template.QuestionTemplateID}-{word.WordID}";
            if (triedCombinations.Contains(key))
            {
                if (triedCombinations.Count >= templates.Count * words.Count)
                {
                    messages.Add("所有可能的題目組合已用完");
                    break; // 沒有更多新題目可以生成
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
                QuestionInstanceID = Guid.NewGuid().ToString(),
                QuestionTemplateID = template.QuestionTemplateID,
                
                QuestionContent = content,
                CreateDate = DateTime.Now
            };

            // 生成選項
            var options = new List<QuestionOption>();
            // 正確答案
            options.Add(new QuestionOption
            {
                QuestionInstanceID = questionInstance.QuestionInstanceID,
                OptionID = Guid.NewGuid().ToString(),
                OptionContent = word.Vocabulary
            });

            // 幹擾選項
            var otherWords = words.Where(w => w.WordID != word.WordID)
                                  .OrderBy(x => _random.Next())
                                  .Take(3);
            foreach (var w in otherWords)
            {
                options.Add(new QuestionOption
                {
                    QuestionInstanceID = questionInstance.QuestionInstanceID,
                    OptionID = Guid.NewGuid().ToString(),
                    OptionContent = w.Vocabulary
                });
            }

            // 打亂順序
            questionInstance.QuestionOption = options.OrderBy(x => _random.Next()).ToList();

            // 存入資料庫
            _context.QuestionInstance.Add(questionInstance);
            await _context.SaveChangesAsync();

            generated++;
            messages.Add($"生成題目：{content}");
        }

        return (generated, messages);
    }
}
