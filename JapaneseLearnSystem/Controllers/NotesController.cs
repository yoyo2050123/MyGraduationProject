using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Models;

namespace JapaneseLearnSystem.Controllers
{
    public class NotesController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public NotesController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var dbJapaneseLearnSystemContext = _context.Note.Include(n => n.JLPTLevel).Include(n => n.Member);
            return View(await dbJapaneseLearnSystemContext.ToListAsync());
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .Include(n => n.JLPTLevel)
                .Include(n => n.Member)
                .FirstOrDefaultAsync(m => m.NoteID == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            ViewData["JLPTLevelID"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelID");
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteID,Title,OriginalArticle,Translate,JLPTLevelID,MemberID")] Note note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JLPTLevelID"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelID", note.JLPTLevelID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", note.MemberID);
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["JLPTLevelID"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelID", note.JLPTLevelID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", note.MemberID);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoteID,Title,OriginalArticle,Translate,JLPTLevelID,MemberID")] Note note)
        {
            if (id != note.NoteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.NoteID))
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
            ViewData["JLPTLevelID"] = new SelectList(_context.JLPTLevel, "JLPTLevelID", "JLPTLevelID", note.JLPTLevelID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", note.MemberID);
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .Include(n => n.JLPTLevel)
                .Include(n => n.Member)
                .FirstOrDefaultAsync(m => m.NoteID == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            if (note != null)
            {
                _context.Note.Remove(note);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.NoteID == id);
        }
    }
}
