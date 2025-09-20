using System;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Models;
using JapaneseLearnSystem.Services;
using Xunit;

namespace JapaneseLearnSystem.Tests
{
    public class MemberUsageServiceTests
    {
        private dbJapaneseLearnSystemContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<dbJapaneseLearnSystemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new dbJapaneseLearnSystemContext(options);

            // Seed 測試資料
            context.SubscriptionPlan.Add(new SubscriptionPlan
            {
                PlanID = 1,
                PlanName = "Free",
                FeeInfo = "免費方案",
                DailyQuestionLimit = 3,
                LearnedWordLimit = 10,
                NoteLimit = 5
            });

            context.Member.Add(new Member
            {
                MemberID = "M001",
                Name = "Test User",
                PlanID = 1,
                Tel = "0912345678",
                Email = "test@example.com",
                Birthday = DateTime.Parse("2000-01-01")
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public void Use_Should_UpdateUsage_When_WithinLimits()
        {
            var context = GetInMemoryContext();
            var service = new MemberUsageService(context);

            // 使用 2 個單字、1 筆記、1 題目
            service.Use("M001", wordsUsed: 2, notesUsed: 1, questionsUsed: 1);

            var usage = context.MemberUsageLog.FirstOrDefault(u => u.MemberID == "M001");
            Assert.NotNull(usage);
            Assert.Equal(2, usage.WordCount);
            Assert.Equal(1, usage.NoteCount);
            Assert.Equal(1, usage.QuestionCount);
        }

        [Fact]
        public void Use_Should_Throw_When_ExceedWordLimit()
        {
            var context = GetInMemoryContext();
            var service = new MemberUsageService(context);

            // 超過單字限制 (Free 限 10)
            Assert.Throws<Exception>(() => service.Use("M001", wordsUsed: 20));
        }

        [Fact]
        public void Use_Should_Throw_When_ExceedNoteLimit()
        {
            var context = GetInMemoryContext();
            var service = new MemberUsageService(context);

            // 超過筆記限制 (Free 限 5)
            Assert.Throws<Exception>(() => service.Use("M001", notesUsed: 10));
        }

        [Fact]
        public void Use_Should_Throw_When_ExceedQuestionLimit()
        {
            var context = GetInMemoryContext();
            var service = new MemberUsageService(context);

            // 超過題目限制 (Free 限 3)
            Assert.Throws<Exception>(() => service.Use("M001", questionsUsed: 5));
        }
    }
}
