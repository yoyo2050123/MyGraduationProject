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
    public class JLPTLevelsController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public JLPTLevelsController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        // GET: JLPTLevels
        public async Task<IActionResult> Index()
        {
            return View(await _context.JLPTLevel.ToListAsync());
        }

        // GET: JLPTLevels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jLPTLevel = await _context.JLPTLevel
                .FirstOrDefaultAsync(m => m.JLPTLevelID == id);
            if (jLPTLevel == null)
            {
                return NotFound();
            }

            return View(jLPTLevel);
        }

        // GET: JLPTLevels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JLPTLevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JLPTLevelID,JLPTLevelName,Description")] JLPTLevel jLPTLevel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jLPTLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jLPTLevel);
        }

        // GET: JLPTLevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jLPTLevel = await _context.JLPTLevel.FindAsync(id);
            if (jLPTLevel == null)
            {
                return NotFound();
            }
            return View(jLPTLevel);
        }

        // POST: JLPTLevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JLPTLevelID,JLPTLevelName,Description")] JLPTLevel jLPTLevel)
        {
            if (id != jLPTLevel.JLPTLevelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jLPTLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JLPTLevelExists(jLPTLevel.JLPTLevelID))
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
            return View(jLPTLevel);
        }

        // GET: JLPTLevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jLPTLevel = await _context.JLPTLevel
                .FirstOrDefaultAsync(m => m.JLPTLevelID == id);
            if (jLPTLevel == null)
            {
                return NotFound();
            }

            return View(jLPTLevel);
        }

        // POST: JLPTLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jLPTLevel = await _context.JLPTLevel.FindAsync(id);
            if (jLPTLevel != null)
            {
                _context.JLPTLevel.Remove(jLPTLevel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JLPTLevelExists(int id)
        {
            return _context.JLPTLevel.Any(e => e.JLPTLevelID == id);
        }
    }
}
