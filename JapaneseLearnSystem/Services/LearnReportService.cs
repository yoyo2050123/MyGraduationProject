using JapaneseLearnSystem.Areas.Members.Models;
using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Services
{
    public class LearnReportService
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public LearnReportService(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        private async Task<string> GenerateRecordIdAsync()
        {
            var lastId = await _context.LearnRecordTable
                                       .OrderByDescending(r => r.RecordID)
                                       .Select(r => r.RecordID)
                                       .FirstOrDefaultAsync();

            int number = 1;
            if (!string.IsNullOrEmpty(lastId) && lastId.Length > 1)
            {
                if (int.TryParse(lastId.Substring(1), out int n))
                {
                    number = n + 1;
                }
            }

            return $"R{number:D4}";
        }

        public async Task<LearnRecordTable> CreateLearnReportAsync(string memberId, int totalAnswers, int correctAnswers, int learnedWordCount = 0)
        {
            decimal accuracy = totalAnswers > 0 ? (decimal)correctAnswers / totalAnswers * 100 : 0;

            var record = new LearnRecordTable
            {
                RecordID = await GenerateRecordIdAsync(),
                MemberID = memberId,
                LearnedWordCount = learnedWordCount,
                TotalAnswers = totalAnswers,
                CorrectAnswers = correctAnswers,
                Accuracy = Math.Round((decimal)correctAnswers / totalAnswers * 100, 2),
                AnswerTime = DateTime.Now
            };

            _context.LearnRecordTable.Add(record);
            await _context.SaveChangesAsync();

            return record;
        }
    }

}
