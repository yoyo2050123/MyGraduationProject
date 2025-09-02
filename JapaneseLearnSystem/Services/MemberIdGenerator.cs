using JapaneseLearnSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearnSystem.Services
{
    public class MemberIdGenerator
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public MemberIdGenerator(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAsync()
        {
            var lastMember = await _context.Member
                .OrderByDescending(m => m.MemberID)
                .FirstOrDefaultAsync();

            int newNumber = 1;
            if (lastMember != null)
            {
                string lastNumberStr = lastMember.MemberID.Substring(1);
                newNumber = int.Parse(lastNumberStr) + 1;
            }

            return "A" + newNumber.ToString("D3");
        }
    }

}
