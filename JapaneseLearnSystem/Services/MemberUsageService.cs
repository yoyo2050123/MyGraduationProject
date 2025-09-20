using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Models;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Services
{
    public class MemberUsageService
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public MemberUsageService(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 更新會員使用量，如果超過限制會拋出例外
        /// </summary>
        public void Use(
            string memberId,
            int wordsUsed = 0,
            int notesUsed = 0,
            int questionsUsed = 0)
        {


            // 1️⃣ 取得會員與方案
            var memberWithPlan = _context.Member
                .Include(m => m.Plan) // 載入 Navigation Property
                .FirstOrDefault(m => m.MemberID == memberId);


            if (memberWithPlan == null) throw new Exception("會員不存在");

            // 2️⃣ 懶惰初始化當天使用紀錄
            var today = DateOnly.FromDateTime(DateTime.Today);

            var usageLog = _context.MemberUsageLog
                .FirstOrDefault(u => u.MemberID == memberId && u.UsageLogDate == today);

            if (usageLog == null)
            {
                usageLog = new MemberUsageLog
                {
                    UsageLogID = "U" + Guid.NewGuid().ToString("N").Substring(0, 6),
                    MemberID = memberId,
                    UsageLogDate = today,
                    WordCount = 0,
                    NoteCount = 0,
                    QuestionCount = 0,
                    CreatedAt = DateTime.Now
                };
                _context.MemberUsageLog.Add(usageLog);
                _context.SaveChanges();
            }

            // 3️⃣ 計算剩餘流量
            int? remainingWords = memberWithPlan.Plan.LearnedWordLimit.HasValue
                ? memberWithPlan.Plan.LearnedWordLimit - usageLog.WordCount
                : (int?)null;

            int? remainingNotes = memberWithPlan.Plan.NoteLimit.HasValue
                ? memberWithPlan.Plan.NoteLimit - usageLog.NoteCount
                : (int?)null;

            int? remainingQuestions = memberWithPlan.Plan.DailyQuestionLimit.HasValue
                ? memberWithPlan.Plan.DailyQuestionLimit - usageLog.QuestionCount
                : (int?)null;

            // 4️⃣ 檢查是否超過限制
            if (remainingWords != null && wordsUsed > remainingWords)
                throw new Exception("單字使用超過限制");
            if (remainingNotes != null && notesUsed > remainingNotes)
                throw new Exception("筆記使用超過限制");
            if (remainingQuestions != null && questionsUsed > remainingQuestions)
                throw new Exception("題目使用超過限制");

            // 5️⃣ 更新累計
            usageLog.WordCount += wordsUsed;
            usageLog.NoteCount += notesUsed;
            usageLog.QuestionCount += questionsUsed;

            _context.SaveChanges();
        }
    }

}
