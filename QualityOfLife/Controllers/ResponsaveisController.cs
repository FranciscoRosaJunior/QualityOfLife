using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Correios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityOfLife.Data;
using QualityOfLife.Models;
using QualityOfLife.Models.Enums;
using System.Configuration;

namespace QualityOfLife.Controllers
{
    public class ResponsaveisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResponsaveisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Responsaveis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Responsavel.ToListAsync());
        }

        // GET: Responsaveis/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responsavel = await _context.Responsavel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (responsavel == null)
            {
                return NotFound();
            }

            return View(responsavel);
        }

        // GET: Responsaveis/Create
        public IActionResult Create()
        {
            //List<string> tipoLogradouro = new List<string>();
            //foreach(var item in Enum.GetValues(typeof(TipoLogradouro)))
            //{
            //    tipoLogradouro.Add(item.ToString());
            //}
            //ViewBag.TipoLogradouro = tipoLogradouro;
            ViewBag.erroCep = "";
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }

        // POST: Responsaveis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Responsavel responsavel)
        {
            if (ModelState.IsValid)
            {
                if(responsavel.Nome != null) responsavel.Nome = responsavel.Nome.ToUpper();
                if(responsavel.Profissao != null) responsavel.Profissao = responsavel.Profissao.ToUpper();
                if(responsavel.Email != null) responsavel.Email = responsavel.Email.ToLower();
                if(responsavel.Rua != null) responsavel.Rua = responsavel.Rua.ToUpper();
                if(responsavel.Complemento != null) responsavel.Complemento = responsavel.Complemento.ToUpper();
                if(responsavel.Bairro != null) responsavel.Bairro = responsavel.Bairro.ToUpper();
                if(responsavel.Cidade != null) responsavel.Cidade = responsavel.Cidade.ToUpper();
                if(responsavel.Estado != null) responsavel.Estado = responsavel.Estado.ToUpper();


                
                _context.Add(responsavel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(responsavel);
        }

        // GET: Responsaveis/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<string> tipoLogradouro = new List<string>();
            foreach (var item in Enum.GetValues(typeof(TipoLogradouro)))
            {
                tipoLogradouro.Add(item.ToString());
            }
            ViewBag.TipoLogradouro = tipoLogradouro;
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var responsavel = await _context.Responsavel.FindAsync(id);
            if (responsavel == null)
            {
                return NotFound();
            }
            return View(responsavel);
        }

        // POST: Responsaveis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Responsavel responsavel)
        {
            if (id != responsavel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (responsavel.Nome != null) responsavel.Nome = responsavel.Nome.ToUpper();
                    if (responsavel.Profissao != null) responsavel.Profissao = responsavel.Profissao.ToUpper();
                    if (responsavel.Email != null) responsavel.Email = responsavel.Email.ToLower();
                    if (responsavel.Rua != null) responsavel.Rua = responsavel.Rua.ToUpper();
                    if (responsavel.Complemento != null) responsavel.Complemento = responsavel.Complemento.ToUpper();
                    if (responsavel.Bairro != null) responsavel.Bairro = responsavel.Bairro.ToUpper();
                    if (responsavel.Cidade != null) responsavel.Cidade = responsavel.Cidade.ToUpper();
                    if (responsavel.Estado != null) responsavel.Estado = responsavel.Estado.ToUpper();
                    _context.Update(responsavel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponsavelExists(responsavel.Id))
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
            return View(responsavel);
        }

        // GET: Responsaveis/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var responsavel = await _context.Responsavel.FindAsync(id);
            _context.Responsavel.Remove(responsavel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var responsavel = await _context.Responsavel
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (responsavel == null)
            //{
            //    return NotFound();
            //}

            //return View(responsavel);
        }

        // POST: Responsaveis/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var responsavel = await _context.Responsavel.FindAsync(id);
        //    _context.Responsavel.Remove(responsavel);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<JsonResult> BuscarCep(string cep)
        {
            cep = cep.Replace("-", "").Replace(".", "");
            var correios = new CorreiosApi();
            var dados = await correios.consultaCEPAsync(cep);

            return Json(dados.@return);
        }

        public async Task<JsonResult> BuscarCpf(string cpf)
        {
            var dados = await _context.Responsavel.Where(x => x.Cpf == cpf).FirstOrDefaultAsync();
            if (dados != null) return Json(dados.Id);
            else return Json(0);
        }

        private bool ResponsavelExists(long id)
        {
            return _context.Responsavel.Any(e => e.Id == id);
        }
    }
}
