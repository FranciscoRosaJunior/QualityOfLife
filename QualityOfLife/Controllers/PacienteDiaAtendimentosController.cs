using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityOfLife.Data;
using QualityOfLife.Models;

namespace QualityOfLife.Controllers
{
    public class PacienteDiaAtendimentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacienteDiaAtendimentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PacienteDiaAtendimentos
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.PacienteDiaAtendimento
        //        .Include(x => x.Paciente)
        //        .ToListAsync());
        //}

        // GET: PacienteDiaAtendimentos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteDiaAtendimento = await _context.PacienteDiaAtendimento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteDiaAtendimento == null)
            {
                return NotFound();
            }

            return View(pacienteDiaAtendimento);
        }

        // GET: PacienteDiaAtendimentos/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Paciente = await _context.Paciente.ToListAsync();
            return View();
        }

        // POST: PacienteDiaAtendimentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(PacienteDiaAtendimento pacienteDiaAtendimento, string cpf)
        //{
        //    var model = new PacienteDiaAtendimento
        //    {
        //        Paciente = await _context.Paciente
        //            .Where(x => x.Cpf == cpf)
        //            .FirstOrDefaultAsync(),
        //        DiaDaSemana = pacienteDiaAtendimento.DiaDaSemana,
        //        Horario = pacienteDiaAtendimento.Horario
        //    };
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(model);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(pacienteDiaAtendimento);
        //}

        // GET: PacienteDiaAtendimentos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteDiaAtendimento = await _context.PacienteDiaAtendimento.FindAsync(id);
            if (pacienteDiaAtendimento == null)
            {
                return NotFound();
            }
            return View(pacienteDiaAtendimento);
        }

        // POST: PacienteDiaAtendimentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DiaDaSemana,Horario")] PacienteDiaAtendimento pacienteDiaAtendimento)
        {
            if (id != pacienteDiaAtendimento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacienteDiaAtendimento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteDiaAtendimentoExists(pacienteDiaAtendimento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            return View(pacienteDiaAtendimento);
        }

        // GET: PacienteDiaAtendimentos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteDiaAtendimento = await _context.PacienteDiaAtendimento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteDiaAtendimento == null)
            {
                return NotFound();
            }

            return View(pacienteDiaAtendimento);
        }

        // POST: PacienteDiaAtendimentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pacienteDiaAtendimento = await _context.PacienteDiaAtendimento.FindAsync(id);
            _context.PacienteDiaAtendimento.Remove(pacienteDiaAtendimento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit));
        }

        private bool PacienteDiaAtendimentoExists(long id)
        {
            return _context.PacienteDiaAtendimento.Any(e => e.Id == id);
        }
    }
}
