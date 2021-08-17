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
        public IActionResult Faturamento()
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
                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, $"Relatório Faturamento " + mes.ToString("MMMM-yyyy")+".xlsx");
            }
        }
        public byte[] Relatorio(ICollection<FaturamentoViewModels> faturamentos, string mes)
        {
            using (var file = new ExcelPackage())
            {
                double total = 0.00;
                int row = 3;
                ExcelWorksheet worksheet = file.Workbook.Worksheets.Add($"{mes.ToUpper()}");
                worksheet.Row(1).Height = 30;
                //Cabeçalho linha 1 mesclado de A a H
                var titulo = worksheet.Cells[1, 1, 1, 8];
                titulo.Merge = true;
                titulo.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                titulo.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                titulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titulo.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
                titulo.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
                titulo.Style.Font.Bold = true;
                titulo.Style.Font.Size = 16;
                titulo.Value = $"Relatório de Atendimento {mes}".ToUpper();

                worksheet.Cells[2, 1, 2, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[2, 1, 2, 8].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#a8a8a8"));
                foreach (var faturamento in faturamentos)
                {
                    var coll = RelatorioHeader(worksheet, row);

                    ICollection<Paciente> paciente = new List<Paciente>();
                    paciente = faturamento.Pacientes;
                    ICollection<Agenda> agenda = new List<Agenda>();
                    agenda = faturamento.Agendas;
                    ICollection<Pedido> pedido = new List<Pedido>();
                    pedido = faturamento.Pedidos;

                    var linhaFinal = RelatorioBody(worksheet, row + 1, paciente, agenda, pedido);
                    //Linha de espaço maior que as demais
                    worksheet.Row(linhaFinal + 1).Height = 50;
                    worksheet.Cells[linhaFinal + 1, 1, linhaFinal + 1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[linhaFinal + 1, 1, linhaFinal + 1, 8].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#a8a8a8"));


                    var subtotal = pedido.Select(x => x.Total).FirstOrDefault();

                    total = total + subtotal;

                    row = linhaFinal + 2;

                }
                worksheet.Cells[row, 1].Value = "VALOR TOTAL RECEBIDO";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#0000ff"));
                worksheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 2].Value = total;
                worksheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
                worksheet.Cells[row, 2].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Style.Font.Size = 14;
                worksheet.Cells[row, 2].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#0000ff"));
                worksheet.Cells[row, 2].Style.Numberformat.Format = "_-R$* #,##0.00_-;-R$* #,##0.00_-;_-R$* \"-\"??_-;_-@_-";
                worksheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 3, row, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 3, row, 8].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#a8a8a8"));
                worksheet.Cells[row +1, 1, row+1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row +1, 1, row+1, 8].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#a8a8a8"));


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
                    worksheet.Cells[index, 1].Value = paciente.Select(x => x.Responsavel.Nome);
                    worksheet.Cells[index, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    
                    worksheet.Cells[index, 2].Value = paciente.Select(x => x.Responsavel.Cpf);
                    worksheet.Cells[index, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    
                    worksheet.Cells[index, 3].Value = datas.DataHora;
                    worksheet.Cells[index, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                    worksheet.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    worksheet.Cells[index, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    nomes++;
                }
                else if (nomes == 2)
                {
                    worksheet.Cells[index, 1].Value = paciente.Select(x => x.Nome);
                    worksheet.Cells[index, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                    worksheet.Cells[index, 2].Value = paciente.Select(x => x.Cpf);
                    worksheet.Cells[index, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                    worksheet.Cells[index, 3].Value = datas.DataHora;
                    worksheet.Cells[index, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                    worksheet.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    worksheet.Cells[index, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    nomes++;
                }
                else
                {
                    worksheet.Cells[index, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[index, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    worksheet.Cells[index, 3].Value = datas.DataHora;
                    worksheet.Cells[index, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                    worksheet.Cells[index, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    
                }
                worksheet.Cells[index, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[index, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[index, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[index, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[index, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                var meioTabela = worksheet.Cells[index, 1, index, 8];
                meioTabela.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                meioTabela.Style.Fill.PatternType = ExcelFillStyle.Solid;
                meioTabela.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffffff"));
                meioTabela.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            //Borda linha Total Por Paciente
            var bordaTotalPaciente = worksheet.Cells[index, 1, index, 8];
            bordaTotalPaciente.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            bordaTotalPaciente.Style.Fill.PatternType = ExcelFillStyle.Solid;
            bordaTotalPaciente.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            bordaTotalPaciente.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));

            //Campo Total Por Paciente
            var TotalPaciente = worksheet.Cells[index, 1, index, 2];
            TotalPaciente.Merge = true;
            TotalPaciente.Style.Font.Bold = true;
            TotalPaciente.Style.Font.Size = 14;
            TotalPaciente.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            TotalPaciente.Value = "Total Por Paciente";
            TotalPaciente.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //Campo Total
            worksheet.Cells[index, 4].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#0000ff"));
            worksheet.Cells[index, 4].Value = pedido.Select(x => x.Total);
            worksheet.Cells[index, 4].Style.Numberformat.Format = "_-R$* #,##0.00_-;-R$* #,##0.00_-;_-R$* \"-\"??_-;_-@_-";
            worksheet.Cells[index, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

            //Campo data do recebimento
            worksheet.Cells[index, 5].Value = pedido.Select(x => x.DataPagamento);
            worksheet.Cells[index, 5].Style.Numberformat.Format = "dd/mm/yyyy";
            worksheet.Cells[index, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            //validar campo Forma Pagamento
            var formaPag = GetEnumValues<FormaPagamento>();
            worksheet.Cells[index, 6].Value = formaPag[pedido.Select(x => x.FormaPagamento).FirstOrDefault() - 1].ToString();
            worksheet.Cells[index, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

            //validar campo Local Pagamento
            var localPag = GetEnumValues<LocalPagamento>();
            worksheet.Cells[index, 7].Value = localPag[pedido.Select(x => x.LocalPagamento).FirstOrDefault() - 1].ToString();
            worksheet.Cells[index, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

            //Validar campo recibo
            string colunaRecibo = pedido.Select(x => x.ReciboEmitido).FirstOrDefault().ToString();
            if (colunaRecibo == "False") colunaRecibo = "Não";
            else colunaRecibo = "Sim";
            worksheet.Cells[index, 8].Value = colunaRecibo;
            worksheet.Cells[index, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

            //Formatar tamanho das colunas
            int coluna = 1;
            worksheet.Column(coluna).Width = 35;
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
            clientes.Style.Fill.PatternType = ExcelFillStyle.Solid;
            clientes.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            clientes.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            clientes.Style.Font.Size = 11;
            clientes.Style.Font.Bold = true;
            clientes.Value = "Paciente/Responsável";
            clientes.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            clientes.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

            int coll = 1;
            row++;

            var Nome = worksheet.Cells[row, coll];
            Nome.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            Nome.Style.Fill.PatternType = ExcelFillStyle.Solid;
            Nome.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            Nome.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            Nome.Value = "Nome";
            Nome.Style.Font.Bold = true;
            Nome.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            coll++;
            worksheet.Cells[row, coll].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, coll].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            worksheet.Cells[row, coll].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            worksheet.Cells[row, coll].Value = "CPF";
            worksheet.Cells[row, coll].Style.Font.Bold = true;
            worksheet.Cells[row, coll].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            coll++;

            var dataAtendimento = worksheet.Cells[row - 1, coll, row, coll];
            dataAtendimento.Merge = true;
            dataAtendimento.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            dataAtendimento.Style.Fill.PatternType = ExcelFillStyle.Solid;
            dataAtendimento.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            dataAtendimento.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            dataAtendimento.Style.Font.Size = 11;
            dataAtendimento.Style.Font.Bold = true;
            dataAtendimento.Value = "Data do Atendimento";
            dataAtendimento.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            coll++;

            var Valor = worksheet.Cells[row - 1, coll, row, coll];
            Valor.Merge = true;
            Valor.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            Valor.Style.Fill.PatternType = ExcelFillStyle.Solid;
            Valor.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            Valor.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            Valor.Style.Font.Size = 11;
            Valor.Style.Font.Bold = true;
            Valor.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            Valor.Value = "Valor Recebido";
            coll++;

            var dataRecebimento = worksheet.Cells[row - 1, coll, row, coll];
            dataRecebimento.Merge = true;
            dataRecebimento.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            dataRecebimento.Style.Fill.PatternType = ExcelFillStyle.Solid;
            dataRecebimento.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            dataRecebimento.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            dataRecebimento.Style.Font.Size = 11;
            dataRecebimento.Style.Font.Bold = true;
            dataRecebimento.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            dataRecebimento.Value = "Data do Recebimento";
            coll++;

            var formaRecebimento = worksheet.Cells[row - 1, coll, row, coll];
            formaRecebimento.Merge = true;
            formaRecebimento.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            formaRecebimento.Style.Fill.PatternType = ExcelFillStyle.Solid;
            formaRecebimento.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            formaRecebimento.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            formaRecebimento.Style.Font.Size = 11;
            formaRecebimento.Style.Font.Bold = true;
            formaRecebimento.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            formaRecebimento.Value = "Forma do Recebido";
            coll++;

            var localRecebimento = worksheet.Cells[row - 1, coll, row, coll];
            localRecebimento.Merge = true;
            localRecebimento.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            localRecebimento.Style.Fill.PatternType = ExcelFillStyle.Solid;
            localRecebimento.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            localRecebimento.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            localRecebimento.Style.Font.Size = 11;
            localRecebimento.Style.Font.Bold = true;
            localRecebimento.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            localRecebimento.Value = "Local do Recebimento";
            coll++;

            var recibo = worksheet.Cells[row - 1, coll, row, coll];
            recibo.Merge = true;
            recibo.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            recibo.Style.Fill.PatternType = ExcelFillStyle.Solid;
            recibo.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff00ff"));
            recibo.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
            recibo.Value = "Recibo Emitido";
            recibo.Style.Font.Size = 11;
            recibo.Style.Font.Bold = true;
            recibo.Style.Border.BorderAround(ExcelBorderStyle.Thin);


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
