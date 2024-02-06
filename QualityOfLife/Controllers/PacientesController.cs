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
using QualityOfLife.Interfaces.IRepositories.IAgenda;
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
                .Where(x => x.StatusPacientes < (StatusPaciente)4)
                .OrderBy(x => x.Nome)
                .Include(x => x.Responsavel)
                .Include(x => x.PacienteDiaAtendimento)
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nome, string cpf, string responsavel, DateTime dataNascimento, StatusPaciente status,
                List<string> listDataHora)
        {
            var paciente = new Paciente();
            try
            {

                paciente = new Paciente
                {
                    Criado = User.Identity.Name,
                    CriadoData = DateTime.Now,
                    Nome = nome.ToUpper(),
                    Cpf = cpf,
                    DataNascimento = dataNascimento,
                    Responsavel = await _context.Responsavel.Where(x => x.Cpf == responsavel).FirstOrDefaultAsync(),
                    StatusPacientes = status
                };
                _context.Add(paciente);
                await _context.SaveChangesAsync();


                var pacienteCriado = await _context.Paciente
                    .Where(x => x.Cpf == cpf)
                    .FirstOrDefaultAsync();

                foreach (var item in listDataHora)
                {
                    string[] diaHora = item.Split('-');
                    var diaHoraAtendimento = new PacienteDiaAtendimento
                    {
                        DiaDaSemana = diaHora[0],
                        Horario = Convert.ToDateTime(diaHora[1]),
                        Paciente = pacienteCriado
                    };
                    _context.Add(diaHoraAtendimento);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(paciente);
            }
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Atendimento = 2;
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.Responsavel = await _context.Responsavel.ToListAsync();
            List<string> StatusPaciente = new List<string>
            {
                "Nenhum",
                "Prospeccao",
                "Avaliacao",
                "Ativo",
                "Inativo"
            };
            ViewBag.StatusPaciente = StatusPaciente;
            var paciente = await _context.Paciente
                .Include(x => x.PacienteDiaAtendimento)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
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
        public async Task<IActionResult> Edit(long id, string nome, string cpf, string responsavel, DateTime dataNascimento, StatusPaciente status,
                List<string> listDataHora)
        {
            var Paciente = await _context.Paciente
                .Include(x => x.PacienteDiaAtendimento)
                .FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                
                Paciente.Modificado = User.Identity.Name;
                Paciente.ModificadoData = DateTime.Now;
                Paciente.Nome = nome.ToUpper();
                Paciente.Cpf = cpf;
                Paciente.DataNascimento = dataNascimento;
                Paciente.Responsavel = await _context.Responsavel.FirstOrDefaultAsync(x => x.Cpf == responsavel);
                Paciente.StatusPacientes = status;
                _context.Update(Paciente);
                await _context.SaveChangesAsync();


                foreach(var i in Paciente.PacienteDiaAtendimento)
                {
                    _context.PacienteDiaAtendimento.Remove(i);
                    
                }

                await _context.SaveChangesAsync();

                foreach (var item in listDataHora)
                {
                    string[] diaHora = item.Split('-');
                    var diaHoraAtendimento = new PacienteDiaAtendimento
                    {
                        DiaDaSemana = diaHora[0],
                        Horario = Convert.ToDateTime(diaHora[1]),
                        Paciente = Paciente
                    };
                    _context.Add(diaHoraAtendimento);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(Paciente);
            }
        }

        // POST: Pacientes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var paciente = await _context.Paciente.FindAsync(id);
            paciente.StatusPacientes = (StatusPaciente)4; //Inativo = 4
            _context.Paciente.Update(paciente);
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
