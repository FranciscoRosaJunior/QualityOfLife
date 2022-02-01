using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultorioTO.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityOfLife.Data;
using QualityOfLife.Models;
using QualityOfLife.Models.ViewModels;
using QualityOfLife.Services;

namespace QualityOfLife.Controllers
{
    public class AgendasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AgendaService _agendaServ;

        public AgendasController(ApplicationDbContext context, AgendaService agendaServ)
        {
            _context = context;
            _agendaServ = agendaServ;
        }

        public async Task<JsonResult> GetEvents()
        {
            List<CalendarioViewModels> calendario = new List<CalendarioViewModels>();
            List<Agenda> listAgenda = new List<Agenda>();

            var agendas = await _context.Agenda
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .ToListAsync();

            foreach (var agenda in agendas)
            {
                calendario.Add(new CalendarioViewModels()
                {
                    DataHora = agenda.DataHora,
                    Anotações = agenda.Anotações,
                    NomePaciente = await _context.Paciente.Where(x => x.Id == agenda.Paciente.Id).Select(x => x.Nome).FirstOrDefaultAsync(),
                    NomeProfissional = await _context.Profissional.Where(x => x.Id == agenda.Profissional.Id).Select(x => x.Nome).FirstOrDefaultAsync()
                });
            }
            
            return Json(calendario.OrderBy(x => x.DataHora));
        }

        // GET: Agendas
        public async Task<IActionResult> Index()
        {
            var agendas = await _context.Agenda
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .ToListAsync();

            return View(agendas.OrderByDescending(x => x.DataHora));
        }

        // GET: Agendas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agenda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // GET: Agendas/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.Paciente = await _context.Paciente.ToListAsync();
            ViewBag.Profissional = await _context.Profissional.ToListAsync();
            return View();
        }

        // POST: Agendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                
                agenda.Valor = agenda.Valor + ",00";
                agenda.Paciente = await _context.Paciente.Where(x => x.Cpf == agenda.Paciente.Cpf).FirstOrDefaultAsync();
                agenda.Profissional = await _context.Profissional.Where(x => x.Cpf == agenda.Profissional.Cpf).FirstOrDefaultAsync();

                if (agenda.Repetir > 0)
                {
                    var datas = _agendaServ.BuscaDatas(agenda.Repetir, agenda.DataHora);

                    foreach (var item in datas)
                    {
                        var ag = new Agenda
                        {
                            Criado = agenda.Criado,
                            CriadoData = agenda.CriadoData,
                            Profissional = agenda.Profissional,
                            DataHora = item,
                            Local = agenda.Local,
                            Paciente = agenda.Paciente,
                            TipoAtendimento = agenda.TipoAtendimento,
                            Presenca = agenda.Presenca,
                            FaltaJustificada = agenda.FaltaJustificada,
                            Falta = agenda.Falta,
                            Reagendar = agenda.Reagendar,
                            Anotações = agenda.Anotações,
                            Repetir = agenda.Repetir,
                            Valor = agenda.Valor
                        };
                        _context.Add(ag);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.Add(agenda);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(agenda);
        }

        // GET: Agendas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.Paciente = await _context.Paciente.ToListAsync();
            ViewBag.Profissional = await _context.Profissional.ToListAsync();
            var agenda = await _context.Agenda.FindAsync(id);
            if (agenda == null)
            {
                return NotFound();
            }
            return View(agenda);
        }

        // POST: Agendas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Agenda agenda)
        {
            if (id != agenda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    agenda.Profissional = await _context.Profissional.FirstOrDefaultAsync(x => x.Cpf == agenda.Profissional.Cpf);
                    agenda.Paciente = await _context.Paciente.FirstOrDefaultAsync(x => x.Cpf == agenda.Paciente.Nome);
                    _context.Update(agenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendaExists(agenda.Id))
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
            return View(agenda);
        }

        // GET: Agendas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agenda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // POST: Agendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var agenda = await _context.Agenda.FindAsync(id);
            _context.Agenda.Remove(agenda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgendaExists(long id)
        {
            return _context.Agenda.Any(e => e.Id == id);
        }
    }
}
