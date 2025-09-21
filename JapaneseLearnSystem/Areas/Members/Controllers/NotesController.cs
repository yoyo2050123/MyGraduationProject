using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(NoteViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var note = new Note
                {
                    Title = vm.Title,
                    OriginalArticle = vm.OriginalArticle,
                    Reading = vm.Reading,
                    Translate = vm.Translate,
                    JLPTLevelID = vm.JLPTLevelID,
                    MemberID = User.FindFirst("MemberID")?.Value
                };

                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["JLPTLevels"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelName", vm.JLPTLevelID);
            return View(vm);
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
                _context.Note.Remove(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
