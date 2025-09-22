using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

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

        // GET: Members/Notes
        public async Task<IActionResult> Index()
        {
            string memberId = User.FindFirst("MemberID")?.Value;

            var member = await _context.Member
                .Include(m => m.Plan)
                .FirstOrDefaultAsync(m => m.MemberID == memberId);

            if (member == null) return Unauthorized();

            int noteCount = await _context.Note.CountAsync(n => n.MemberID == member.MemberID);

            // 計算剩餘筆數
            string notesRemaining;
            if (member.Plan?.NoteLimit.HasValue == true)
            {
                notesRemaining = (member.Plan.NoteLimit.Value - noteCount).ToString();
            }
            else
            {
                notesRemaining = "無限制";
            }
            ViewBag.NotesRemaining = notesRemaining;

            // 取得筆記列表
            var notes = await _context.Note
                .Include(n => n.JLPTLevel)
                .Where(n => n.MemberID == memberId)
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

            return View(notes);
        }



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
            if (ModelState.IsValid)
            {
                var member = _context.Member
                    .Include(m => m.Plan)
                    .FirstOrDefault(m => m.MemberID == memberId);

                if (member == null)
                    return Unauthorized();

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

                // ===== 更新 MemberUsageLog =====
                var today = DateOnly.FromDateTime(DateTime.Today);
                var usage = _context.MemberUsageLog
                    .FirstOrDefault(u => u.MemberID == member.MemberID && u.UsageLogDate == today);

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

            ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName", model.JLPTLevelID);
            return View(model);
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

            if (ModelState.IsValid)
            {
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
                    if (!_context.Note.Any(e => e.NoteID == vm.NoteID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName", vm.JLPTLevelID);
            return View(vm);
        }

        // GET: Members/Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

                // ===== 更新 MemberUsageLog =====
                var today = DateOnly.FromDateTime(DateTime.Today);
                var usage = _context.MemberUsageLog
                    .FirstOrDefault(u => u.MemberID == memberId && u.UsageLogDate == today);

                if (usage != null && usage.NoteCount > 0)
                {
                    usage.NoteCount -= 1;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
