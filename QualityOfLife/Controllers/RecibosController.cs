using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityOfLife.Data;
using QualityOfLife.Models;
using PdfSharpCore.Pdf;
using Microsoft.AspNetCore.Authorization;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using QualityOfLife.Models.Enums;
using QualityOfLife.Models.ViewModels;
using QualityOfLife.Services;

namespace QualityOfLife.Controllers
{
    public class RecibosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReciboService _servico;

        public RecibosController(ApplicationDbContext context, ReciboService servico)
        {
            _context = context;
            _servico = servico;
        }

        //GerarPdf
        public async Task<FileResult> GerarRecibo(long? id)
        {
            var pedido = await _context.Pedido
                .Where(x => x.Id == id)
                .Include(x => x.Paciente)
                .Include(x => x.Paciente.Responsavel)
                .Include(x => x.Profissional)
                .FirstOrDefaultAsync();

            var responsavel = await _context.Responsavel.Where(x => x.Cpf == pedido.Paciente.Responsavel.Cpf).FirstOrDefaultAsync();

            var agendas = await _context.AgendaPedidos
                .Where(x => x.Pedido.Id == pedido.Id)
                .Include(x => x.Agenda)
                .ToListAsync();
            var document = await _servico.PdfRecibo(responsavel, pedido, agendas);

            string paciente = pedido.Paciente.Nome.Substring(0, pedido.Paciente.Nome.IndexOf(' '));
            string resp = pedido.Paciente.Responsavel.Nome.Substring(0, pedido.Paciente.Responsavel.Nome.IndexOf(' '));
            string mes = pedido.mesreferencia.Substring(5, 2);
            string ano = pedido.mesreferencia.Substring(0, 4);
            var nomeArquivo = paciente + "-" + resp + " RB" + pedido.Id.ToString().PadLeft(5, '0') + " " + mes + "-" + ano + ".pdf";

            byte[] bytes = null;

            using (MemoryStream stream = new MemoryStream())
            {
                var contantType = "application/pdf";
                document.Save(stream, false);
                return File(stream.ToArray(), contantType, nomeArquivo);
            }

        }


    }
}
