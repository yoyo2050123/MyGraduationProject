using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Models;

namespace JapaneseLearnSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionManagementsController : Controller
    {
        private readonly dbJapaneseLearnSystemContextG2 _context;

        public QuestionManagementsController(dbJapaneseLearnSystemContextG2 context)
        {
            _context = context;
        }

        // GET: Admin/QuestionManagement
        public async Task<IActionResult> Index()
        {
            var dbJapaneseLearnSystemContext = _context.QuestionInstance.Include(q => q.QuestionTemplate);
            return View(await dbJapaneseLearnSystemContext.ToListAsync());
        }

        // GET: Admin/QuestionManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionInstance = await _context.QuestionInstance
                .Include(q => q.QuestionTemplate)
                .FirstOrDefaultAsync(m => m.QuestionInstanceID == id);
            if (questionInstance == null)
            {
                return NotFound();
            }

            return View(questionInstance);
        }

        // GET: Admin/QuestionManagement/Create
        public IActionResult Create()
        {
            ViewData["QuestionTemplateID"] = new SelectList(_context.QuestionTemplate, "QuestionTemplateID", "QuestionTemplateID");
            return View();
        }

        // POST: Admin/QuestionManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionInstanceID,QuestionTemplateID,AnswerOptionID,QuestionContent,CreateDate")] QuestionInstance questionInstance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionInstance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionTemplateID"] = new SelectList(_context.QuestionTemplate, "QuestionTemplateID", "QuestionTemplateID", questionInstance.QuestionTemplateID);
            return View(questionInstance);
        }

        // GET: Admin/QuestionManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionInstance = await _context.QuestionInstance.FindAsync(id);
            if (questionInstance == null)
            {
                return NotFound();
            }
            ViewData["QuestionTemplateID"] = new SelectList(_context.QuestionTemplate, "QuestionTemplateID", "QuestionTemplateID", questionInstance.QuestionTemplateID);
            return View(questionInstance);
        }

        // POST: Admin/QuestionManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("QuestionInstanceID,QuestionTemplateID,AnswerOptionID,QuestionContent,CreateDate")] QuestionInstance questionInstance)
        {
            if (id != questionInstance.QuestionInstanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionInstance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionInstanceExists(questionInstance.QuestionInstanceID))
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
            ViewData["QuestionTemplateID"] = new SelectList(_context.QuestionTemplate, "QuestionTemplateID", "QuestionTemplateID", questionInstance.QuestionTemplateID);
            return View(questionInstance);
        }

        // GET: Admin/QuestionManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionInstance = await _context.QuestionInstance
                .Include(q => q.QuestionTemplate)
                .FirstOrDefaultAsync(m => m.QuestionInstanceID == id);
            if (questionInstance == null)
            {
                return NotFound();
            }

            return View(questionInstance);
        }

        // POST: Admin/QuestionManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var questionInstance = await _context.QuestionInstance.FindAsync(id);
            if (questionInstance != null)
            {
                _context.QuestionInstance.Remove(questionInstance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionInstanceExists(string id)
        {
            return _context.QuestionInstance.Any(e => e.QuestionInstanceID == id);
        }
    }
}
