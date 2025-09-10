using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace JapaneseLearnSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly dbJapaneseLearnSystemContextG2 _context;

        public AccountController(dbJapaneseLearnSystemContextG2 context)
        {
            _context = context;
        }

        // GET: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["LogoutMsg"] = "已登出！";
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // 記住要回跳的位置（未登入被擋時會用到）
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {


                var user = _context.MemberAccount
                    .Include(ma => ma.Member)
                        .ThenInclude(m => m.MemberRole)   // 經由 Member 拿到 MemberRole
                            .ThenInclude(mr => mr.Role)   // 再取 Role
                    .FirstOrDefault(ma => ma.Account == model.Account);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    // 建立 Claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Account),
                        new Claim("MemberID", user.MemberID)
                    };

                    // 🚀 把角色寫進 Claims
                    // 加入角色 Claim
                    foreach (var memberRole in user.Member.MemberRole)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, memberRole.Role.RoleName));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                         new ClaimsPrincipal(claimsIdentity)
                    );

                    TempData["Message"] = "登入成功！";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    // 如果是管理員，直接進入後台
                    if (user.Member.MemberRole.Any(mr => mr.Role.RoleName == "管理員"))
                        return RedirectToAction("Index", "MemberManagements", new { area = "Admin" });


                    // 其他人進入首頁
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["LoginError"] = "帳號或密碼錯誤！";
                    return View(model);
                }

            }
                
            return View(model);
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
                var existingAccount = _context.MemberAccount.FirstOrDefault(m => m.Account == model.Account);
                var existingEmail = _context.Member.FirstOrDefault(m => m.Email == model.Email);
                if (existingAccount != null)
                {
                    ModelState.AddModelError(string.Empty, "這個 帳號 已經被註冊過了。");
                    return View(model);
                }   

                if (existingEmail != null)
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
                // 3) 雜湊密碼（不要明碼存！）
                var hashed = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // 建立 MemberAccount 物件 (對應到 MemberAccount 資料表)
                var memberAccount = new MemberAccount
                {
                    Account = model.Account,
                    Password = hashed,  // 存雜湊值
                    MemberID = newMemberId
                };

                // 將兩個物件都加入到資料庫追蹤中
                _context.Member.Add(member);
                _context.MemberAccount.Add(memberAccount);

                // ---------------------------
                // 在這裡建立 MemberRole
                var defaultRole = await _context.Role.FirstOrDefaultAsync(r => r.RoleName == "一般會員");

                if (defaultRole != null)
                {
                    var memberRole = new MemberRole
                    {
                        MemberID = newMemberId,
                        RoleID = defaultRole.RoleID
                    };
                    _context.MemberRole.Add(memberRole); // 👈 別忘了這行
                }


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
