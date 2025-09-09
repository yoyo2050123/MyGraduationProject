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
    public class MemberManagementsController : Controller
    {
        private readonly dbJapaneseLearnSystemContextG2 _context;

        public MemberManagementsController(dbJapaneseLearnSystemContextG2 context)
        {
            _context = context;
        }

        // GET: Admin/MemberManagement
        public async Task<IActionResult> Index()
        {
            var dbJapaneseLearnSystemContext = _context.Member.Include(m => m.Plan);
            return View(await dbJapaneseLearnSystemContext.ToListAsync());
        }

        // GET: Admin/MemberManagement/Details/5
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

        // GET: Admin/MemberManagement/Create
        public IActionResult Create()
        {
            ViewData["PlanID"] = new SelectList(_context.SubscriptionPlan, "PlanID", "PlanID");
            return View();
        }

        // POST: Admin/MemberManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberID,Name,Tel,PlanID,Email,Birthday")] JapaneseLearnSystem.Models.Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanID"] = new SelectList(_context.SubscriptionPlan, "PlanID", "PlanID", member.PlanID);
            return View(member);
        }

        // GET: Admin/MemberManagement/Edit/5
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

        // POST: Admin/MemberManagement/Edit/5
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

        // GET: Admin/MemberManagement/Delete/5
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

        // POST: Admin/MemberManagement/Delete/5
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
