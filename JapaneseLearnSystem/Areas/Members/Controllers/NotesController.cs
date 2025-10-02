using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using JapaneseLearnSystem.Areas.Members.Models;

namespace JapaneseLearnSystem.Areas.Members.Controllers
{
    [Area("Members")]
    [Authorize(Roles = "一般會員,付費會員")]
    public class NotesController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public NotesController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        #region Index & Ajax 分頁
        // GET: Members/Notes
        public async Task<IActionResult> Index()
        {
            string memberId = User.FindFirst("MemberID")?.Value;
            var vm = await GetNotesPage(memberId, 1); // 初始第1頁
            ViewBag.NotesRemaining = await GetRemainingNotes(memberId);
            return View(vm);
        }

        // 共用分頁邏輯（新增 search 參數），最小改動：只在這裡把搜尋併入 query
        private async Task<NoteListViewModel> GetNotesPage(string memberId, int page, string? search = null)
        {
            const int pageSize = 20; // 你原本固定 20
                                     // 基礎 Query（含 JLPTLevel）
            var query = _context.Note
                        .Include(n => n.JLPTLevel)
                        .Where(n => n.MemberID == memberId);

            // 如果有搜尋字串則加上條件（Title 或 OriginalArticle）
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(n => n.Title.Contains(search) || n.OriginalArticle.Contains(search));
            }

            var totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var notes = await query
                .OrderBy(n => n.NoteID) // 你想要最早寫的筆記先出現
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NoteViewModel
                {
                    NoteID = n.NoteID,
                    Title = n.Title,
                    OriginalArticle = n.OriginalArticle,
                    Reading = n.Reading,
                    Translate = n.Translate,
                    JLPTLevelID = n.JLPTLevelID,
                    JLPTLevelName = n.JLPTLevel.JLPTLevelName,
                    MemberID = n.MemberID
                })
                .ToListAsync();

            return new NoteListViewModel
            {
                Notes = notes,
                CurrentPage = page,
                TotalPages = totalPages,
                Search = search
            };
        }
        // Ajax Partial：支援 page 與 search
        public async Task<IActionResult> NotesPartial(int page = 1, string? search = null)
        {
            string memberId = User.FindFirst("MemberID")?.Value;
            var vm = await GetNotesPage(memberId, page, search);
            ViewBag.NotesRemaining = await GetRemainingNotes(memberId);
            return PartialView("_NotesTable", vm);
        }

        // 共用：取得分頁資料
        private async Task<NoteListViewModel> GetNotesPage(string memberId, int page)
        {
            int pageSize = 10;
            var totalCount = await _context.Note.CountAsync(n => n.MemberID == memberId);
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var notes = await _context.Note
                .Include(n => n.JLPTLevel)
                .Where(n => n.MemberID == memberId)
                .OrderBy(n => n.NoteID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NoteViewModel
                {
                    NoteID = n.NoteID,
                    Title = n.Title,
                    OriginalArticle = n.OriginalArticle,
                    Reading = n.Reading,
                    Translate = n.Translate,
                    JLPTLevelID = n.JLPTLevelID,
                    JLPTLevelName = n.JLPTLevel.JLPTLevelName,
                    MemberID = n.MemberID
                })
                .ToListAsync();

            return new NoteListViewModel
            {
                Notes = notes,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }

        // Helper: 取得剩餘可新增筆記數
        private async Task<string> GetRemainingNotes(string memberId)
        {
            var member = await _context.Member.Include(m => m.Plan)
                                              .FirstOrDefaultAsync(m => m.MemberID == memberId);
            if (member == null) return "無法取得";

            int noteCount = await _context.Note.CountAsync(n => n.MemberID == memberId);

            if (member.Plan?.NoteLimit.HasValue == true)
                return (member.Plan.NoteLimit.Value - noteCount).ToString();
            else
                return "無限制";
        }
        #endregion

        #region CRUD
        // GET: Members/Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var note = await _context.Note
                .Include(n => n.JLPTLevel)
                .FirstOrDefaultAsync(n => n.NoteID == id);

            if (note == null) return NotFound();

            var vm = new NoteViewModel
            {
                NoteID = note.NoteID,
                Title = note.Title,
                OriginalArticle = note.OriginalArticle,
                Reading = note.Reading,
                Translate = note.Translate,
                JLPTLevelID = note.JLPTLevelID,
                JLPTLevelName = note.JLPTLevel.JLPTLevelName,
                MemberID = note.MemberID
            };

            return View(vm);
        }

        // GET: Members/Notes/Create
        public IActionResult Create()
        {
            ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName");
            return View();
        }

        // POST: Members/Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoteViewModel model)
        {
            string memberId = User.FindFirst("MemberID")?.Value;
            if (!ModelState.IsValid)
            {
                ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName", model.JLPTLevelID);
                return View(model);
            }

            var member = _context.Member.Include(m => m.Plan)
                .FirstOrDefault(m => m.MemberID == memberId);

            if (member == null) return Unauthorized();

            int noteCount = _context.Note.Count(n => n.MemberID == member.MemberID);
            int noteLimit = member.Plan?.NoteLimit ?? int.MaxValue;

            if (noteCount >= noteLimit)
            {
                ModelState.AddModelError("", "已達到筆記數量上限。");
                ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName", model.JLPTLevelID);
                return View(model);
            }

            var note = new Note
            {
                Title = model.Title,
                OriginalArticle = model.OriginalArticle,
                Reading = model.Reading,
                Translate = model.Translate,
                JLPTLevelID = model.JLPTLevelID,
                MemberID = member.MemberID
            };
            _context.Note.Add(note);

            // 更新 MemberUsageLog
            var today = DateOnly.FromDateTime(DateTime.Today);
            var usage = _context.MemberUsageLog.FirstOrDefault(u => u.MemberID == member.MemberID && u.UsageLogDate == today);

            if (usage == null)
            {
                usage = new MemberUsageLog
                {
                    UsageLogID = $"U{member.MemberID}{today:yyyyMMdd}",
                    MemberID = member.MemberID,
                    UsageLogDate = today,
                    NoteCount = 1,
                    WordCount = 0,
                    QuestionCount = 0,
                    CreatedAt = DateTime.Now
                };
                _context.MemberUsageLog.Add(usage);
            }
            else
            {
                usage.NoteCount += 1;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Members/Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var note = await _context.Note.FindAsync(id);
            if (note == null) return NotFound();

            var vm = new NoteViewModel
            {
                NoteID = note.NoteID,
                Title = note.Title,
                OriginalArticle = note.OriginalArticle,
                Reading = note.Reading,
                Translate = note.Translate,
                JLPTLevelID = note.JLPTLevelID,
                MemberID = note.MemberID
            };

            ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName", vm.JLPTLevelID);
            return View(vm);
        }

        // POST: Members/Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteViewModel vm)
        {
            if (id != vm.NoteID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName", vm.JLPTLevelID);
                return View(vm);
            }

            try
            {
                var note = await _context.Note.FindAsync(id);
                if (note == null) return NotFound();

                note.Title = vm.Title;
                note.OriginalArticle = vm.OriginalArticle;
                note.Reading = vm.Reading;
                note.Translate = vm.Translate;
                note.JLPTLevelID = vm.JLPTLevelID;

                _context.Update(note);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Note.Any(e => e.NoteID == vm.NoteID)) return NotFound();
                else throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Members/Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var note = await _context.Note.Include(n => n.JLPTLevel)
                .FirstOrDefaultAsync(n => n.NoteID == id);

            if (note == null) return NotFound();

            var vm = new NoteViewModel
            {
                NoteID = note.NoteID,
                Title = note.Title,
                OriginalArticle = note.OriginalArticle,
                Reading = note.Reading,
                Translate = note.Translate,
                JLPTLevelID = note.JLPTLevelID,
                JLPTLevelName = note.JLPTLevel.JLPTLevelName,
                MemberID = note.MemberID
            };

            return View(vm);
        }

        // POST: Members/Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            if (note != null)
            {
                var memberId = note.MemberID;
                _context.Note.Remove(note);

                // 更新 MemberUsageLog
                // 更新 MemberUsageLog
                var today = DateOnly.FromDateTime(DateTime.Today);
                var usage = _context.MemberUsageLog.FirstOrDefault(u => u.MemberID == memberId && u.UsageLogDate == today);

                if (usage != null && usage.NoteCount > 0)
                {
                    usage.NoteCount -= 1;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

