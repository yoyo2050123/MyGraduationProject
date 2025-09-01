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
    public class SubscriptionPlansController : Controller
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public SubscriptionPlansController(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        // GET: SubscriptionPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubscriptionPlan.ToListAsync());
        }

        // GET: SubscriptionPlans/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionPlan = await _context.SubscriptionPlan
                .FirstOrDefaultAsync(m => m.PlanID == id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }

            return View(subscriptionPlan);
        }

        // GET: SubscriptionPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubscriptionPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanID,PlanName,FeeInfo,DailyQuestionLimit,LearnedWordLimit,FavoriteLimit")] SubscriptionPlan subscriptionPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscriptionPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subscriptionPlan);
        }

        // GET: SubscriptionPlans/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionPlan = await _context.SubscriptionPlan.FindAsync(id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }
            return View(subscriptionPlan);
        }

        // POST: SubscriptionPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("PlanID,PlanName,FeeInfo,DailyQuestionLimit,LearnedWordLimit,FavoriteLimit")] SubscriptionPlan subscriptionPlan)
        {
            if (id != subscriptionPlan.PlanID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriptionPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionPlanExists(subscriptionPlan.PlanID))
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
            return View(subscriptionPlan);
        }

        // GET: SubscriptionPlans/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionPlan = await _context.SubscriptionPlan
                .FirstOrDefaultAsync(m => m.PlanID == id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }

            return View(subscriptionPlan);
        }

        // POST: SubscriptionPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            var subscriptionPlan = await _context.SubscriptionPlan.FindAsync(id);
            if (subscriptionPlan != null)
            {
                _context.SubscriptionPlan.Remove(subscriptionPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionPlanExists(byte id)
        {
            return _context.SubscriptionPlan.Any(e => e.PlanID == id);
        }
    }
}
