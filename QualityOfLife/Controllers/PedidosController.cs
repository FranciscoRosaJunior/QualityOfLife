using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using QualityOfLife.Data;
using QualityOfLife.Models;
using QualityOfLife.Models.Enums;
using QualityOfLife.Models.ViewModels;
using QualityOfLife.Services;

namespace QualityOfLife.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PedidoService _servico;

        public PedidosController(ApplicationDbContext context, PedidoService service)
        {
            _context = context;
            _servico = service;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pedido
                .Include(x => x.Paciente)
                .OrderByDescending(x => x.DataPedido)
                .ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(x => x.Paciente)
                .Include(x => x.Paciente.Responsavel)
                .Include(x => x.Profissional)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido.Pagamento == true)
            {
                var localPag = GetEnumValues<LocalPagamento>();
                ViewBag.local = localPag[pedido.LocalPagamento - 1].ToString();
                var formaPag = GetEnumValues<FormaPagamento>();
                ViewBag.forma = formaPag[pedido.FormaPagamento - 1].ToString();
            }
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }
        public static T[] GetEnumValues<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                return null;
            }
            return (T[])Enum.GetValues(typeof(T));
        }

        // GET: Pedidos/Create
        public async Task<IActionResult> Create(string paciente, string mes)
        {
            ViewBag.cpf = paciente;
            ViewBag.Mes = mes;
            ViewBag.Paciente = await _context.Paciente.ToListAsync();
            if (paciente == null)
            {
                List<PedidoAgenda> PedidoAgenda = new List<PedidoAgenda>();
                return View(PedidoAgenda);
            }
            else
            {
                var model = await _context.Agenda.Where(x => x.Paciente.Cpf == paciente)
                    .Where(x => x.DataHora.ToString()
                    .Contains(mes)).Include(x => x.Profissional).ToListAsync();
                List<PedidoAgenda> PedidoAgenda = new List<PedidoAgenda>();
                foreach (var agenda in model)
                {
                    PedidoAgenda.Add(new PedidoAgenda()
                    {
                        Escolher = true,
                        Agenda = agenda

                    });
                }

                return View(PedidoAgenda);
            }

        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<JsonResult> CreatePost(Pedido pedido, string pacientePost, string mesPost, List<int> idsAgenda)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Agenda.Where(x => idsAgenda.Contains(Convert.ToInt16(x.Id)))
                    .Include(x => x.Profissional)
                    .Include(x => x.Paciente)
                    .ToListAsync();

                string mesAnterior = model.Select(x => x.DataHora.AddMonths(-1)).FirstOrDefault().ToString("MM/yyyy");

                var presenca = model.Where(x => x.Presenca == true);
                var faltaJustificada = await _context.Agenda
                    .Where(x => x.Paciente.Cpf == pacientePost && x.DataHora.ToString("MM/yyyy").Contains(mesAnterior) && x.FaltaJustificada == true)
                    .ToListAsync();
                int i = 1;

                foreach (var item in presenca)
                {
                    pedido.Valor = pedido.Valor + Convert.ToDouble(item.Valor);
                }
                foreach (var item in faltaJustificada)
                {
                    pedido.Credito = pedido.Credito + Convert.ToDouble(item.Valor);
                    pedido.Observacoes = "Possui " + i + " Atendimento(s) de crédito.";
                    i++;
                }

                pedido.CriadoData = DateTime.Now;
                pedido.Criado = User.Identity.Name;
                pedido.DataPedido = DateTime.Now;
                pedido.mesreferencia = mesPost;
                //pedido.Desconto = 100.00;
                pedido.Total = pedido.Valor + pedido.Desconto - pedido.Credito;
                pedido.Paciente = model.Select(x => x.Paciente).FirstOrDefault();
                pedido.Profissional = model.Select(x => x.Profissional).FirstOrDefault();

                

                _context.Add(pedido);
                await _context.SaveChangesAsync();

                var pedidoGerado = await _context.Pedido.LastOrDefaultAsync();

                //popular tabela AgendaPedido
                foreach (var item in model)
                {
                    AgendaPedido agendaPedido = new AgendaPedido();
                    agendaPedido.Agenda = item;
                    agendaPedido.Pedido = pedidoGerado;

                    _context.Add(agendaPedido);
                    await _context.SaveChangesAsync();
                }
                return Json(true);
            }
            return Json(false);
        }

        //GerarPdf
        public async Task<FileResult> GerarPdf2(long? id)
        {
            var pedido = await _context.Pedido
                .Where(x => x.Id == id)
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .FirstOrDefaultAsync();

            var document = await _servico.PdfPedido(pedido);

            string paciente = pedido.Paciente.Nome.Substring(0, pedido.Paciente.Nome.IndexOf(' '));
            string resp = pedido.Paciente.Responsavel.Nome.Substring(0, pedido.Paciente.Responsavel.Nome.IndexOf(' '));
            string mes = pedido.mesreferencia.Substring(5, 2);
            string ano = pedido.mesreferencia.Substring(0, 4);
            var nomeArquivo = paciente + "-" + resp + " NF" + pedido.Id.ToString().PadLeft(5, '0') + " " + mes + "-" + ano + ".pdf";

            byte[] bytes = null;

            using (MemoryStream stream = new MemoryStream())
            {
                var contantType = "application/pdf";
                document.Save(stream, false);
                return File(stream.ToArray(), contantType, nomeArquivo);
            }

        }









        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CurrentUser = User.Identity.Name;
            ViewBag.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            var agendapedido = await _context.AgendaPedidos.Where(x => x.Pedido.Id == id).ToListAsync();

            foreach(var item in agendapedido)
            {
                _context.AgendaPedidos.Remove(item);
                await _context.SaveChangesAsync();
            }
            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(long id)
        {
            return _context.Pedido.Any(e => e.Id == id);
        }
    }
}
