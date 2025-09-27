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

        var triedCombinations = new HashSet<string>();

        while (generated < count)
        {
            var template = templates[_random.Next(templates.Count)];
            var word = words[_random.Next(words.Count)];

            string content = template.QuestionTemplateText.Replace("{0}", word.Vocabulary);
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

            bool exists = await _context.QuestionInstance.AnyAsync(q => q.QuestionContent == content);
            if (exists) continue;

            var questionInstance = new QuestionInstance
            {
                QuestionInstanceID = await GenerateQuestionInstanceIdAsync(),
                QuestionTemplateID = template.QuestionTemplateID,
                WordID = word.WordID,
                QuestionContent = content,
                CreateDate = DateTime.Now
            };

            // 正確答案
            string correctOptionContent = template.QuestionType switch
            {
                "Reading" => word.Reading,
                "Meaning" => word.WordTranslate,
                _ => word.Vocabulary
            };

            var optionsSet = new HashSet<string>();
            optionsSet.Add(correctOptionContent);

            // 1. 先建立選項（正確答案 + 幹擾選項）
            var options = new List<QuestionOption>
            {
                new QuestionOption
                {
                    QuestionInstanceID = questionInstance.QuestionInstanceID,
                    OptionID = "1", // 正確答案暫時編號
                    OptionContent = correctOptionContent
                }
            };

            var shuffledWords = words.Where(w => w.WordID != word.WordID)
                                     .OrderBy(x => _random.Next())
                                     .ToList();

            int idx = 2;
            foreach (var w in shuffledWords)
            {
                string optionContent = template.QuestionType switch
                {
                    "Reading" => w.Reading,
                    "Meaning" => w.WordTranslate,
                    _ => w.Vocabulary
                };

                if (optionsSet.Contains(optionContent)) continue;

                optionsSet.Add(optionContent);
                options.Add(new QuestionOption
                {
                    QuestionInstanceID = questionInstance.QuestionInstanceID,
                    OptionID = idx.ToString(), // 干擾選項 2~4
                    OptionContent = optionContent
                });

                idx++;
                if (options.Count >= 4) break; // 共 4 個選項
            }

            // 2. 打亂選項順序
            var shuffledOptions = options.OrderBy(x => _random.Next()).ToList();

            // 3. 設定正確答案對應打亂後的 OptionID
            var correctOption = shuffledOptions.First(o => o.OptionContent == correctOptionContent);
            questionInstance.AnswerOptionID = correctOption.OptionID;

            // 4. 儲存打亂後的選項
            questionInstance.QuestionOption = shuffledOptions;

            // 5. 存入資料庫
            _context.QuestionInstance.Add(questionInstance);
            await _context.SaveChangesAsync();

            generated++;
            messages.Add($"生成題目：{content} | 正確答案：{correctOptionContent}");
        }

        return (generated, messages);
    }

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
