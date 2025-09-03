using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearnSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public AccountController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 去 MemberAccount 資料表檢查帳號密碼
                var user = _context.MemberAccount
                    .FirstOrDefault(m => m.Account == model.Account && m.Password == model.Password);

                if (user != null)
                {
                    // TODO: 建立 session 或 cookie
                    TempData["Message"] = "登入成功！";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.Clear();
                TempData["LoginError"] = "帳號或密碼錯誤！";
            }

            return View(model);
        }
    }
}
