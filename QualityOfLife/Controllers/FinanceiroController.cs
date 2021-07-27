using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QualityOfLife.Data;
using QualityOfLife.Models;
using QualityOfLife.Models.Enums;
using QualityOfLife.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QualityOfLife.Controllers
{
    public class FinanceiroController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinanceiroController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Faturamento()
        {
            ICollection<FaturamentoViewModels> model = new List<FaturamentoViewModels>();
            return View(model);
        }

        [HttpPost]
        public async Task<FileResult> GerarFaturamento(DateTime mes)
        {
            string mesRef = mes.ToString("yyyy-MM");
            if (mes == null)
            {
                ICollection<FaturamentoViewModels> model = new List<FaturamentoViewModels>();
                var result = Relatorio(model, mes.ToString());
                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, $"Relatório Faturamento.xlsx");
            }
            else
            {
                ViewBag.Mes = mes;

                ICollection<FaturamentoViewModels> model = new List<FaturamentoViewModels>();

                var pacientesList = await _context.Paciente
                    .Where(x => x.StatusPacientes.ToString() == "3")
                    .Include(x => x.Responsavel)
                    .ToListAsync();

                foreach (var paciente in pacientesList)
                {
                    ICollection<Paciente> pacientes = new List<Paciente>();
                    pacientes.Add(paciente);

                    var pedido = await _context.Pedido
                        .Where(x => x.Paciente.Id == paciente.Id)
                        .Where(x => x.mesreferencia == mesRef)
                        .Where(x => x.Pagamento == true)
                        .ToListAsync();

                    if (pedido.Count > 0)
                    {
                        var agenda = await _context.Agenda
                        .Where(x => x.Paciente.Id == paciente.Id)
                        .Where(x => x.DataHora.ToString("yyyy-MM") == mesRef)
                        .ToListAsync();

                        model.Add(new FaturamentoViewModels()
                        {
                            Pacientes = pacientes,
                            Pedidos = pedido,
                            Agendas = agenda,
                        });
                    }
                }

                var result = Relatorio(model, mes.ToString("MMMM/yyyy"));
                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, $"Relatório Faturamento.xlsx");
            }
        }
        public byte[] Relatorio(ICollection<FaturamentoViewModels> faturamentos, string mes)
        {
            using (var file = new ExcelPackage())
            {
                double total = 0.00;
                int row = 3;
                ExcelWorksheet worksheet = file.Workbook.Worksheets.Add($"{mes}");
                //Cabeçalho linha 1 mesclado de A a H
                var titulo = worksheet.Cells[1, 1, 1, 8];
                titulo.Merge = true;
                titulo.Style.Font.Bold = true;
                titulo.Style.Font.Size = 12;
                titulo.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                titulo.Value = $"Relatório de Atendimento {mes}";
                foreach (var faturamento in faturamentos)
                {
                    var coll = RelatorioHeader(worksheet, row);

                    ICollection<Paciente> paciente = new List<Paciente>();
                    paciente = faturamento.Pacientes;
                    ICollection<Agenda> agenda = new List<Agenda>();
                    agenda = faturamento.Agendas;
                    ICollection<Pedido> pedido = new List<Pedido>();
                    pedido = faturamento.Pedidos;

                    var linhaFinal = RelatorioBody(worksheet, row + 2, paciente, agenda, pedido);

                    var subtotal = pedido.Select(x => x.Total).FirstOrDefault();

                    total = total + subtotal;

                    row = linhaFinal + 2;

                }
                worksheet.Cells[row, 1].Value = "VALOR TOTAL RECEBIDO";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 2].Value = total;
                worksheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0000ff"));
                worksheet.Cells[row, 2].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Style.Font.Size = 14;
                worksheet.Cells[row, 2].Style.Numberformat.Format = "_-R$* #,##0.00_-;-R$* #,##0.00_-;_-R$* \"-\"??_-;_-@_-";

                file.Compression = CompressionLevel.BestCompression;
                return file.GetAsByteArray();
            }
        }

        private int RelatorioBody(ExcelWorksheet worksheet, int row, ICollection<Paciente> paciente, ICollection<Agenda> agenda, ICollection<Pedido> pedido)
        {

            int nomes = 1;
            int index = 0;
            row++;
            for (index = row; index < agenda.Count() + row; index++)
            {
                var datas = agenda.ElementAt(index - row);
                if (nomes == 1)
                {
                    worksheet.Cells[index, 1].Value = paciente.Select(x => x.Nome);
                    worksheet.Cells[index, 2].Value = paciente.Select(x => x.Cpf);
                    worksheet.Cells[index, 3].Value = datas.DataHora;
                    worksheet.Cells[index, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                    nomes++;
                }
                else if (nomes == 2)
                {
                    worksheet.Cells[index, 1].Value = paciente.Select(x => x.Responsavel.Nome);
                    worksheet.Cells[index, 2].Value = paciente.Select(x => x.Responsavel.Cpf);
                    worksheet.Cells[index, 3].Value = datas.DataHora;
                    worksheet.Cells[index, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                    nomes++;
                }
                else
                {
                    worksheet.Cells[index, 3].Value = datas.DataHora;
                    worksheet.Cells[index, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                }

            }
            //Campo Total
            worksheet.Cells[index, 4].Value = pedido.Select(x => x.Total);
            worksheet.Cells[index, 4].Style.Numberformat.Format = "_-R$* #,##0.00_-;-R$* #,##0.00_-;_-R$* \"-\"??_-;_-@_-";
            
            //Campo data do recebimento
            worksheet.Cells[index, 5].Value = pedido.Select(x => x.DataPagamento);
            worksheet.Cells[index, 5].Style.Numberformat.Format = "dd/mm/yyyy";

            //validar campo Forma Pagamento
            var formaPag = GetEnumValues<FormaPagamento>();
            worksheet.Cells[index, 6].Value = formaPag[pedido.Select(x => x.FormaPagamento).FirstOrDefault() - 1].ToString();

            //validar campo Local Pagamento
            var localPag = GetEnumValues<LocalPagamento>();
            worksheet.Cells[index, 7].Value = localPag[pedido.Select(x => x.LocalPagamento).FirstOrDefault() - 1].ToString();
            
            //Validar campo recibo
            string colunaRecibo = pedido.Select(x => x.ReciboEmitido).FirstOrDefault().ToString();
            if (colunaRecibo == "False") colunaRecibo = "Não";
            else colunaRecibo = "Sim";
            worksheet.Cells[index, 8].Value = colunaRecibo;

            //Formatar tamanho das colunas
            int coluna = 1;
            worksheet.Column(coluna).Width = 40;
            for (int col = 2; col <= 8; col++)
            {
                worksheet.Column(col).Width = 20;
            }

            return index;
        }

        /// <summary>
        /// Cria o header da tabela
        private static int RelatorioHeader(ExcelWorksheet worksheet, int row)
        {
            //Cabeçalho linha 1 mesclado de A a H
            var clientes = worksheet.Cells[row, 1, row, 2];
            clientes.Merge = true;
            clientes.Style.Font.Size = 11;
            clientes.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            clientes.Value = "Paciente/Responsável";
            int coll = 1;
            row++;
            worksheet.Cells[row, coll++].Value = "Nome";
            worksheet.Cells[row, coll++].Value = "CPF";

            var dataAtendimento = worksheet.Cells[row - 1, coll, row, coll];
            dataAtendimento.Merge = true;
            dataAtendimento.Style.Font.Size = 11;
            dataAtendimento.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            dataAtendimento.Value = "Data do Atendimento";
            coll++;

            var Valor = worksheet.Cells[row - 1, coll, row, coll];
            Valor.Merge = true;
            Valor.Style.Font.Size = 11;
            Valor.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            Valor.Value = "Valor Recebido";
            coll++;

            var dataRecebimento = worksheet.Cells[row - 1, coll, row, coll];
            dataRecebimento.Merge = true;
            dataRecebimento.Style.Font.Size = 11;
            dataRecebimento.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            dataRecebimento.Value = "Data do Recebimento";
            coll++;

            var formaRecebimento = worksheet.Cells[row - 1, coll, row, coll];
            formaRecebimento.Merge = true;
            formaRecebimento.Style.Font.Size = 11;
            formaRecebimento.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            formaRecebimento.Value = "Forma do Recebido";
            coll++;

            var localRecebimento = worksheet.Cells[row - 1, coll, row, coll];
            localRecebimento.Merge = true;
            localRecebimento.Style.Font.Size = 11;
            localRecebimento.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            localRecebimento.Value = "Local do Recebimento";
            coll++;

            var recibo = worksheet.Cells[row - 1, coll, row, coll];
            recibo.Merge = true;
            recibo.Style.Font.Size = 11;
            recibo.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            recibo.Value = "Recibo Emitido";

            return coll;
        }

        public static T[] GetEnumValues<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                return null;
            }
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
