using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QualityOfLife.Data;
using QualityOfLife.Interfaces;
using QualityOfLife.Interfaces.IRepositories.IFinanceiro;
using QualityOfLife.Models;
using QualityOfLife.Models.Enums;
using QualityOfLife.Models.ViewModels;
using QualityOfLife.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;

namespace QualityOfLife.Controllers
{
    [Authorize]
    public class FinanceiroController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFinanceiroService _financeiroService;
        private readonly IEnvioEmailService _envioEmailService;

        public FinanceiroController(ApplicationDbContext context, IFinanceiroService financeiroService,
            IEnvioEmailService envioEmailService)
        {
            _context = context;
            _financeiroService = financeiroService;
            _envioEmailService = envioEmailService;
        }

        //VisualizaFaturamento
        public async Task<IActionResult> Faturamento(DateTime mes)
        {
            ViewBag.Mes = mes;
            string mesRef = mes.ToString("yyyy-MM");
            //refatorar essa condição. Adicionar obrigatoriedade de preenchimento na view
            if (mesRef != "0001-01")
            {
                ViewBag.Mes = mes;
                var model = await _financeiroService.BuscaFaturamento(mesRef);
                return View(model);

            }
            else
            {
                ICollection<FaturamentoViewModels> model = new List<FaturamentoViewModels>();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<FileResult> GerarFaturamento(DateTime mes)
        {
            string mesRef = mes.ToString("yyyy-MM");
            //refatorar essa condição. Adicionar obrigatoriedade de preenchimento na view
            if (mes != null)
            {
                ViewBag.Mes = mes;
                var result = await _financeiroService.GeraFaturamento(mesRef);
                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, $"Relatório Faturamento " + mes.ToString("MM-yyyy") + ".xlsx");

            }
            else
            {
                ICollection<FaturamentoViewModels> model = new List<FaturamentoViewModels>();
                var result = await _financeiroService.GeraFaturamento(mesRef);
                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, $"Relatório Faturamento.xlsx");
            }
        }
        

        public async Task<String> EnviaFaturamento(DateTime mes, string destino, string assunto, string mensagem)
        {
            string mesRef = mes.ToString("yyyy-MM");
            string Resposta = "Falha ao enviar e-mail";
            if (mes != null)
            {

                ViewBag.Mes = mes;
                var result = await _financeiroService.GeraFaturamento(mesRef);
                string NomeArquivo = $"Relatório Faturamento " + mes.ToString("MM-yyyy") + ".xlsx";

                await _envioEmailService.EnviarEmailAsync(destino, assunto, mensagem, result, NomeArquivo);
                Resposta = "Email enviado com sucesso";
            }
            else
            {
                Resposta = "Falha ao enviar e-mail - Escolha um mês de referência ";
            }

            return Resposta;

        }

    }
}
