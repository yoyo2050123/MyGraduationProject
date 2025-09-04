using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Services;

namespace JapaneseLearnSystem.Areas.Members.Controllers
{
    public class MembersController : Controller
    {

        private readonly dbJapaneseLearnSystemContext _context;
        public IActionResult Index()
        {
            return View();
        }

        // 請將這兩個 Action 方法複製到你的 MembersController 類別裡面

        // GET: /Members/Register
        // 這個 Action 用來顯示註冊的空白表單頁面
        public IActionResult Register()
        {
            return View();
        }


        // POST: /Members/Register
        // 這個 Action 用來接收使用者填寫完畢後送出的表單資料
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ModelState.Remove("MemberID");
            if (ModelState.IsValid)
            {

                Console.WriteLine("Register POST action called");
                // 檢查 Email 是否已經被註冊 (這是正確的)
                var existingMember = _context.MemberAccount.FirstOrDefault(m => m.Account == model.Account);
                if (existingMember != null)
                {
                    ModelState.AddModelError(string.Empty, "這個 Email 已經被註冊過了。");
                    return View(model);
                }

                // 取得資料庫裡最大 ID
                var lastMember = await _context.Member
                    .OrderByDescending(m => m.MemberID)
                    .FirstOrDefaultAsync();

                int newNumber = 1;
                if (lastMember != null)
                {
                    // 取出數字部分並 +1
                    string lastNumberStr = lastMember.MemberID.Substring(1); // 去掉字首 'A'
                    newNumber = int.Parse(lastNumberStr) + 1;
                }

                // 組成新 MemberID，例如 A001、A002
                string newMemberId = "A" + newNumber.ToString("D3"); // D3 表示補 0 到三位數



                // 建立 Member 物件 (對應到 Member 資料表)
                var member = new JapaneseLearnSystem.Models.Member
                {
                    MemberID = newMemberId,
                    Name = model.Name,
                    Tel = model.Tel,
                    Email = model.Email,
                    Birthday = DateOnly.FromDateTime(model.Birthday),
                    PlanID = 1
                };

                // 建立 MemberAccount 物件 (對應到 MemberAccount 資料表)
                var memberAccount = new MemberAccount
                {
                    Account = model.Account,
                    Password = model.Password,
                    MemberID = newMemberId
                };

                // 將兩個物件都加入到資料庫追蹤中
                _context.Member.Add(member);
                _context.MemberAccount.Add(memberAccount);

                // 一次性儲存所有變更
                await _context.SaveChangesAsync();

                // 註冊成功後，將使用者導向登入頁面
                return RedirectToAction("Login");
            }
            // 如果資料驗證失敗，回到原本的註冊頁面並顯示錯誤訊息
            return View(model);
        }
    }
}
