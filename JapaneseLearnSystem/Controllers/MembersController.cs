using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using JapaneseLearnSystem.Services;

namespace JapaneseLearnSystem.Controllers
{
    public class MembersController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;
        private readonly MemberIdGenerator _memberIdGenerator;

        public MembersController(dbJapaneseLearnSystemContext context,MemberIdGenerator memberIdGenerator)
        {
            _context = context;
            _memberIdGenerator = memberIdGenerator;
        }



        // GET: Members
        public async Task<IActionResult> Index()
        {
            var dbJapaneseLearnSystemContext = _context.Member.Include(m => m.Plan);
            return View(await dbJapaneseLearnSystemContext.ToListAsync());
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
                var member = new Member
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

        // GET: Members/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.Plan)
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["PlanID"] = new SelectList(_context.SubscriptionPlan, "PlanID", "PlanID");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberID,Name,Tel,PlanID,Email,Birthday")] Member member)
        {
            ModelState.Remove("MemberID");
            if (ModelState.IsValid)
            {
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
                member.MemberID = "A" + newNumber.ToString("D3"); // D3 表示補 0 到三位數

                try
                {
                    _context.Add(member);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("資料已成功寫入資料庫");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("寫入資料庫失敗：" + ex.Message);
                    // 可選：檢查內部例外 ex.InnerException
                }
                
                
                return RedirectToAction(nameof(Index));

            }
            ViewData["PlanID"] = new SelectList(_context.SubscriptionPlan, "PlanID", "PlanID", member.PlanID);
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["PlanID"] = new SelectList(_context.SubscriptionPlan, "PlanID", "PlanID", member.PlanID);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemberID,Name,Tel,PlanID,Email,Birthday")] Member member)
        {
            if (id != member.MemberID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanID"] = new SelectList(_context.SubscriptionPlan, "PlanID", "PlanID", member.PlanID);
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.Plan)
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.MemberID == id);
        }
    }
}
