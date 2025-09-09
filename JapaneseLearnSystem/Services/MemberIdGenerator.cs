using JapaneseLearnSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearnSystem.Services
{
    public class MemberIdGenerator
    {
        private readonly dbJapaneseLearnSystemContextG2 _context;

        public MemberIdGenerator(dbJapaneseLearnSystemContextG2 context)
        {
            _context = context;
        }

        public async Task<string> GenerateAsync()
        {
            // 取得資料庫裡最大 ID
            var lastMember = await _context.Member
                .OrderByDescending(m => m.MemberID)
                .FirstOrDefaultAsync();

            // 取出數字部分並 +1
            int newMemberId = 1;
            if (lastMember != null)
            {
                string lastNumberStr = lastMember.MemberID.Substring(1);
                newMemberId = int.Parse(lastNumberStr) + 1;
            }

            // 組成新 MemberID，例如 A001、A002
            return "A" + newMemberId.ToString("D3");
        }
    }

}
