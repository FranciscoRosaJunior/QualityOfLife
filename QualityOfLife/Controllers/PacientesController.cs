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

namespace QualityOfLife.Controllers
{
    [Authorize]
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var Model = await _context.Paciente
                .Include(x => x.Responsavel)
                .ToListAsync();
            return View(Model);
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.Responsavel = await _context.Responsavel.ToListAsync();
            List<string> StatusPaciente = new List<string>();
            StatusPaciente.Add("Nenhum");
            StatusPaciente.Add("Prospeccao");
            StatusPaciente.Add("Avaliacao");
            StatusPaciente.Add("Ativo");
            StatusPaciente.Add("Inativo");
            ViewBag.StatusPaciente = StatusPaciente;
            List<string> DiaAtendimentos = new List<string>();
            DiaAtendimentos.Add("Segunda");
            DiaAtendimentos.Add("Terça");
            DiaAtendimentos.Add("Quarta");
            DiaAtendimentos.Add("Quinta");
            DiaAtendimentos.Add("Sexta");
            DiaAtendimentos.Add("Sabado");
            ViewBag.DiaAtendimentos = DiaAtendimentos;
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente, List<string> diaAtendimento)
        {
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (ModelState.IsValid)
            {
                int i = 1;
                paciente.DiaAtendimento = "";
                foreach (string dia in diaAtendimento)
                {

                    if (diaAtendimento.Count > i)
                    {
                        paciente.DiaAtendimento = paciente.DiaAtendimento + dia + ", ";
                        i++;
                    }
                    else
                    {
                        paciente.DiaAtendimento = paciente.DiaAtendimento + dia + ".";
                    }

                }

                paciente.Responsavel = await _context.Responsavel.Where(x => x.Cpf == paciente.Responsavel.Cpf).FirstOrDefaultAsync();
                if (paciente.Nome != null) paciente.Nome = paciente.Nome.ToUpper();
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.Responsavel = await _context.Responsavel.ToListAsync();
            List<string> StatusPaciente = new List<string>();
            StatusPaciente.Add("Nenhum");
            StatusPaciente.Add("Prospeccao");
            StatusPaciente.Add("Avaliacao");
            StatusPaciente.Add("Ativo");
            StatusPaciente.Add("Inativo");
            ViewBag.StatusPaciente = StatusPaciente;
            List<string> DiaAtendimentos = new List<string>();
            DiaAtendimentos.Add("Segunda");
            DiaAtendimentos.Add("Terça");
            DiaAtendimentos.Add("Quarta");
            DiaAtendimentos.Add("Quinta");
            DiaAtendimentos.Add("Sexta");
            DiaAtendimentos.Add("Sabado");
            ViewBag.DiaAtendimentos = DiaAtendimentos;
            var paciente = await _context.Paciente.FindAsync(id);
            paciente.DiaAtendimento = "";
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Paciente paciente, List<string> diaAtendimento)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int i = 1;
                    paciente.DiaAtendimento = "";
                    foreach (string dia in diaAtendimento)
                    {

                        if (diaAtendimento.Count > i)
                        {
                            paciente.DiaAtendimento = paciente.DiaAtendimento + dia + ", ";
                            i++;
                        }
                        else
                        {
                            paciente.DiaAtendimento = paciente.DiaAtendimento + dia + ".";
                        }

                    }
                    paciente.Responsavel = await _context.Responsavel.Where(x => x.Cpf == paciente.Responsavel.Cpf).FirstOrDefaultAsync();
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
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
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var paciente = await _context.Paciente.FindAsync(id);
            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //// POST: Pacientes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var paciente = await _context.Paciente.FindAsync(id);
        //    _context.Paciente.Remove(paciente);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        public async Task<JsonResult> BuscarCpf(string cpf)
        {
            var dados = await _context.Paciente.Where(x => x.Cpf == cpf).FirstOrDefaultAsync();
            if (dados != null) return Json(dados.Id);
            else return Json(0);
        }
        private bool PacienteExists(long id)
        {
            return _context.Paciente.Any(e => e.Id == id);
        }
    }
}
