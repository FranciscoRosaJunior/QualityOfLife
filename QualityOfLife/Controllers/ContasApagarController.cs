using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityOfLife.Data;
using QualityOfLife.Models;

namespace QualityOfLife.Controllers
{
    [Authorize]
    public class ContasApagarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContasApagarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContasApagars
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContasApagar.ToListAsync());
        }

        // GET: ContasApagars/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contasApagar = await _context.ContasApagar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contasApagar == null)
            {
                return NotFound();
            }

            return View(contasApagar);
        }

        // GET: ContasApagars/Create
        public IActionResult Create()
        {
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }

        // POST: ContasApagars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContasApagar contasApagar)
        {
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (ModelState.IsValid)
            {
                _context.Add(contasApagar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contasApagar);
        }

        // GET: ContasApagars/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contasApagar = await _context.ContasApagar.FindAsync(id);
            if (contasApagar == null)
            {
                return NotFound();
            }
            return View(contasApagar);
        }

        // POST: ContasApagars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ContasApagar contasApagar)
        {
            if (id != contasApagar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contasApagar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContasApagarExists(contasApagar.Id))
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
            return View(contasApagar);
        }

        // GET: ContasApagars/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contasApagar = await _context.ContasApagar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contasApagar == null)
            {
                return NotFound();
            }

            return View(contasApagar);
        }

        // POST: ContasApagars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var contasApagar = await _context.ContasApagar.FindAsync(id);
            _context.ContasApagar.Remove(contasApagar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContasApagarExists(long id)
        {
            return _context.ContasApagar.Any(e => e.Id == id);
        }
    }
}
