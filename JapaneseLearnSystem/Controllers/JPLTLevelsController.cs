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
    public class JPLTLevelsController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public JPLTLevelsController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        // GET: JPLTLevels
        public async Task<IActionResult> Index()
        {
            return View(await _context.JPLTLevel.ToListAsync());
        }

        // GET: JPLTLevels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jPLTLevel = await _context.JPLTLevel
                .FirstOrDefaultAsync(m => m.JPLTLevelID == id);
            if (jPLTLevel == null)
            {
                return NotFound();
            }

            return View(jPLTLevel);
        }

        // GET: JPLTLevels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JPLTLevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JPLTLevelID,JPLTLevelName")] JPLTLevel jPLTLevel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jPLTLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jPLTLevel);
        }

        // GET: JPLTLevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jPLTLevel = await _context.JPLTLevel.FindAsync(id);
            if (jPLTLevel == null)
            {
                return NotFound();
            }
            return View(jPLTLevel);
        }

        // POST: JPLTLevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JPLTLevelID,JPLTLevelName")] JPLTLevel jPLTLevel)
        {
            if (id != jPLTLevel.JPLTLevelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jPLTLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JPLTLevelExists(jPLTLevel.JPLTLevelID))
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
            return View(jPLTLevel);
        }

        // GET: JPLTLevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jPLTLevel = await _context.JPLTLevel
                .FirstOrDefaultAsync(m => m.JPLTLevelID == id);
            if (jPLTLevel == null)
            {
                return NotFound();
            }

            return View(jPLTLevel);
        }

        // POST: JPLTLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jPLTLevel = await _context.JPLTLevel.FindAsync(id);
            if (jPLTLevel != null)
            {
                _context.JPLTLevel.Remove(jPLTLevel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JPLTLevelExists(int id)
        {
            return _context.JPLTLevel.Any(e => e.JPLTLevelID == id);
        }
    }
}
