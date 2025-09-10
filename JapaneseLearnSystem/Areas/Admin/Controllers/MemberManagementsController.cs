using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearnSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "管理員")]
    public class MemberManagementsController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public MemberManagementsController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        // GET: Admin/MemberManagements
        public async Task<IActionResult> Index()
        {
            var dbJapaneseLearnSystemContext = _context.Member.Include(m => m.Plan);
            return View(await dbJapaneseLearnSystemContext.ToListAsync());
        }

        // GET: Admin/MemberManagements/Details/5
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

        // GET: Admin/MemberManagements/Create
        public IActionResult Create()
        {
            ViewData["PlanID"] = new SelectList(_context.SubscriptionPlan, "PlanID", "PlanID");
            return View();
        }

        // POST: Admin/MemberManagements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberID,Name,Tel,PlanID,Email,Birthday")] Member member)
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

        // GET: Admin/MemberManagements/Edit/5
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

        // POST: Admin/MemberManagements/Edit/5
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

        // GET: Admin/MemberManagements/Delete/5
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

        // POST: Admin/MemberManagements/Delete/5
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
