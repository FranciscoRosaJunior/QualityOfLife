

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using QualityOfLife.Data;
using QualityOfLife.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Services
{
    public class PedidoService
    {
        private readonly ApplicationDbContext _context;

        public PedidoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PdfDocument> PdfPedido(Pedido pedido)
        {
            long ResponsavelId = await _context.Paciente
                .Where(x => x.Id == pedido.Paciente.Id).Select(x => x.Responsavel.Id).FirstOrDefaultAsync();

            var responsavel = await _context.Responsavel
                .Where(x => x.Id == ResponsavelId).FirstOrDefaultAsync();

            var agenda = await _context.Agenda
                .Where(x => x.Paciente.Id == pedido.Paciente.Id && x.DataHora.ToString().Contains(pedido.mesreferencia) && x.Presenca == true).OrderBy(x => x.DataHora).ToListAsync();

            string mesAnterior = agenda.Select(x => x.DataHora).FirstOrDefault().AddMonths(-1).Month.ToString();

            var datas = await _context.Agenda
                .Where(x => x.Paciente.Id == pedido.Paciente.Id && x.DataHora.Month.ToString().Contains(mesAnterior) && x.FaltaJustificada == true).OrderBy(x => x.DataHora).Select(x => x.DataHora).ToListAsync();

            string linhaDatas = "";

            foreach (var d in datas)
            {
                if (linhaDatas == "")
                {
                    linhaDatas = d.ToString("dd/MM/yyyy");
                }
                else
                {
                    linhaDatas = linhaDatas + ", " + d.ToString("dd/MM/yyyy");
                }

            }

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Pedido " + pedido.Paciente.Nome + " - " + pedido.DataPedido.ToString("yyyyMMddHHmmss");

            for (int p = 0; p < 1; p++)
            {
                // Page Options
                PdfPage pdfPage = document.AddPage();
                pdfPage.Height = 842;//842
                pdfPage.Width = 590;

                // Get an XGraphics object for drawing
                XGraphics graph = XGraphics.FromPdfPage(pdfPage);

                // Text format
                XStringFormat format = new XStringFormat();
                format.LineAlignment = XLineAlignment.Near;
                format.Alignment = XStringAlignment.Near;
                var tf = new XTextFormatter(graph);

                XFont fontCabecalho = new XFont("Arial", 16, XFontStyle.Regular);
                XFont fontParagraph = new XFont("Arial", 12, XFontStyle.Regular);
                XFont fontTabelaNegrito = new XFont("Arial", 12, XFontStyle.Bold);
                XFont fontpedido = new XFont("Arial", 26, XFontStyle.Bold);

                int i = 0;

                // page structure options
                int rect_height = 17;
                int sifrao = 31;
                int sifraoTotal = 20;

                int borda = 23;
                int bordaRetangulo = 3;
                int RetRosaTab = 372;
                int RetBrancoTab = 374;

                double SomaSubtotal = 0.00, SomaTotal = 0;


                XSolidBrush rect_style1 = new XSolidBrush(XColors.White);
                XSolidBrush rect_style2 = new XSolidBrush(XColors.Magenta);

                var logo = @"D:\Francisco\Projetos Gerais\QualityOfLife\QualityOfLife\wwwroot\images\imagempedido.png";

                if (agenda.Count > 0)
                {
                    int xInicial = 20;
                    int yInicial = 20;
                    var larguraPage = pdfPage.Width - 2 * 20;
                    int alturaImagem = 193;

                    //Imagem do cabeçalho
                    XImage imagem = XImage.FromFile(logo);
                    graph.DrawImage(imagem, xInicial, yInicial, larguraPage, alturaImagem);

                    //Escrito pedido
                    tf.DrawString("PEDIDO", fontpedido, XBrushes.Magenta, new XRect(150, 225, larguraPage, 30), format);

                    //Excrito Data
                    tf.DrawString("Data:", fontParagraph, XBrushes.Black, new XRect(394, 225, 980, 30), format);
                    graph.DrawRectangle(rect_style2, 425, 220, 145, 22);//Retangulo Rosa
                    graph.DrawRectangle(rect_style1, 427, 222, 141, 19);//Retangulo Branco
                    tf.DrawString(DateTime.Now.ToString("dd/MM/yyyy"), fontParagraph, XBrushes.Black, new XRect(430, 225, 980, 30), format);

                    //Escrito Número
                    tf.DrawString("Número:", fontParagraph, XBrushes.Black, new XRect(376, 253, 980, 30), format);
                    graph.DrawRectangle(rect_style2, 425, 246, 145, 22);//Retangulo Rosa
                    graph.DrawRectangle(rect_style1, 427, 248, 141, 18);//Retangulo Branco
                    tf.DrawString(DateTime.Now.ToString("dd/MM/yyyy").Substring(6, 4) + "/" + pedido.Id.ToString().PadLeft(5, '0'), fontParagraph, XBrushes.Black, new XRect(430, 251, 980, 30), format);

                    //Cabeçalho de dados do cliente
                    graph.DrawRectangle(rect_style2, 20, 271, pdfPage.Width - 2 * 20, 80);
                    tf.DrawString("Dados do Cliente", fontCabecalho, XBrushes.White, new XRect(230, 271, 145, 17), format);

                    // Informações Cabeçalho de dados do cliente
                    graph.DrawRectangle(rect_style1, 24, 289, pdfPage.Width - 2 * 20 - 7, 60);
                    tf.DrawString("Responsável: " + responsavel.Nome, fontParagraph, XBrushes.Black, new XRect(26, 291, 380, 17), format);
                    tf.DrawString("Criança: " + pedido.Paciente.Nome, fontParagraph, XBrushes.Black, new XRect(26, 305, 380, 17), format);
                    tf.DrawString("Contato: " + responsavel.Telefone1, fontParagraph, XBrushes.Black, new XRect(26, 319, 380, 17), format);
                    tf.DrawString("E-mail: " + responsavel.Email, fontParagraph, XBrushes.Black, new XRect(26, 333, 380, 17), format);

                    //Retangulo Rosa Cabeçalho tabela
                    graph.DrawRectangle(rect_style2, 20, 355, pdfPage.Width - 2 * 20, 17);

                    tf.DrawString("Tabela de Atendimentos", fontCabecalho, XBrushes.White,
                                  new XRect(200, 356, 200, 30), format);

                    graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 22);//387

                    //Primeira Linha e Primeira coluna
                    graph.DrawRectangle(rect_style1, 20 + bordaRetangulo, RetBrancoTab, 125 - borda - bordaRetangulo, rect_height);
                    tf.DrawString("Data", fontParagraph, XBrushes.Black, new XRect(55, RetBrancoTab, 80, 30), format);//25

                    ////ELEMENT 2 - BIG 380
                    graph.DrawRectangle(rect_style1, 125, RetBrancoTab, 146 - borda, rect_height);
                    tf.DrawString("Tipo", fontParagraph, XBrushes.Black, new XRect(172, RetBrancoTab, 380, 30), format);//127

                    ////ELEMENT 3 - SMALL 80
                    graph.DrawRectangle(rect_style1, 250, RetBrancoTab, 195 - borda, rect_height);
                    tf.DrawString("Profissional", fontParagraph, XBrushes.Black, new XRect(302, RetBrancoTab, 680, 30), format);//252

                    ////ELEMENT 4 - SMALL 80
                    graph.DrawRectangle(rect_style1, 425, RetBrancoTab, 145 - bordaRetangulo, rect_height);
                    tf.DrawString("Valor", fontParagraph, XBrushes.Black, new XRect(477, RetBrancoTab, 980, 30), format);//427

                    foreach (var item in agenda)
                    {
                        //item.Valor = 920; //RETIRAR QUANDO JÁ ESTIVER RECEBENDO O VALOR


                        SomaSubtotal = SomaSubtotal + Convert.ToDouble(item.Valor);

                        RetRosaTab = RetRosaTab + 17;
                        RetBrancoTab = RetBrancoTab + 17;

                        graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 22);//387

                        //Primeira Linha e Primeira coluna
                        graph.DrawRectangle(rect_style1, 20 + bordaRetangulo, RetBrancoTab, 125 - borda - bordaRetangulo, rect_height);
                        tf.DrawString(item.DataHora.ToString("dd/MM/yyyy"), fontParagraph, XBrushes.Black, new XRect(25, RetBrancoTab, 80, 30), format);

                        ////ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style1, 125, RetBrancoTab, 146 - borda, rect_height);
                        tf.DrawString(item.TipoAtendimento.ToString(), fontParagraph, XBrushes.Black, new XRect(127, RetBrancoTab, 380, 30), format);

                        ////ELEMENT 3 - SMALL 80
                        graph.DrawRectangle(rect_style1, 250, RetBrancoTab, 195 - borda, rect_height);
                        tf.DrawString(item.Profissional.Nome, fontParagraph, XBrushes.Black, new XRect(252, RetBrancoTab, 680, 30), format);

                        ////ELEMENT 4 - SMALL 80
                        graph.DrawRectangle(rect_style1, 425, RetBrancoTab, 145 - bordaRetangulo, rect_height);

                        string valor = item.Valor;
                        tf.DrawString("R$", fontParagraph, XBrushes.Black, new XRect(425, RetBrancoTab, 980, 30), format);

                        if (valor.Contains(",00")) valor = valor.Replace(",00", "");
                        if (valor.Length > 3)
                        {
                            tf.DrawString(valor + ",00", fontParagraph, XBrushes.Black, new XRect(526, RetBrancoTab, 980, 30), format);
                        }
                        else
                        {
                            tf.DrawString(valor + ",00", fontParagraph, XBrushes.Black, new XRect(530, RetBrancoTab, 980, 30), format);
                        }
                        i++;
                    }

                    //completa a tabela com 10 linhas
                    while (i < 15)
                    {
                        RetRosaTab = RetRosaTab + 17;
                        RetBrancoTab = RetBrancoTab + 17;

                        graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 22);//387

                        //Primeira Linha e Primeira coluna
                        graph.DrawRectangle(rect_style1, 20 + bordaRetangulo, RetBrancoTab, 125 - borda - bordaRetangulo, rect_height);

                        ////ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style1, 125, RetBrancoTab, 146 - borda, rect_height);

                        ////ELEMENT 3 - SMALL 80
                        graph.DrawRectangle(rect_style1, 250, RetBrancoTab, 195 - borda, rect_height);

                        ////ELEMENT 4 - SMALL 80
                        graph.DrawRectangle(rect_style1, 425, RetBrancoTab, 145 - bordaRetangulo, rect_height);
                        i++;
                    }
                    RetRosaTab = RetRosaTab + 25;
                    //
                    graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 22);
                    graph.DrawRectangle(rect_style1, 24, RetRosaTab + 2, 398, 18);
                    tf.DrawString("SUBTOTAL", fontTabelaNegrito, XBrushes.Black, new XRect(26, RetRosaTab + 4, 980, 30), format);
                    graph.DrawRectangle(rect_style1, 425, RetRosaTab + 2, 142, 18);

                    string subtotal = SomaSubtotal.ToString() + ",00";
                    tf.DrawString("R$", fontParagraph, XBrushes.Black, new XRect(425, RetRosaTab + 4, 980, 30), format);

                    if (subtotal.Length > 3)
                    {
                        tf.DrawString(subtotal, fontParagraph, XBrushes.Black, new XRect(524, RetRosaTab + 4, 980, 30), format);
                    }
                    else
                    {
                        tf.DrawString(subtotal, fontParagraph, XBrushes.Black, new XRect(530, RetRosaTab + 4, 980, 30), format);
                    }

                    RetRosaTab = RetRosaTab + 25;
                    graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 66);//Retangulo Rosa Maior
                    graph.DrawRectangle(rect_style1, 24, RetRosaTab + 2, 291, 61);//Retangulo Branco Maior
                    tf.DrawString("Observações", fontTabelaNegrito, XBrushes.Black, new XRect(130, RetRosaTab + 4, 200, 18), format);


                    if (pedido.Observacoes != null)
                    {
                        if (pedido.Observacoes.Length < 43)
                        {
                            tf.DrawString(pedido.Observacoes,//max 42 caracteres
                            fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 24, 398, 14), format);//Linha 1 Observação

                            if (linhaDatas.Length > 0)
                            {
                                if (linhaDatas.Length < 25)
                                {
                                    tf.DrawString("Referente(s) a(s) data(s): " + linhaDatas + ".",
                                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 2 Observação
                                }
                                else
                                {
                                    string linha2 = linhaDatas.Substring(0, 24);
                                    string linha3 = linhaDatas.Substring(linha2.Length, linhaDatas.Length - linha2.Length);
                                    tf.DrawString("Referente(s) a(s) data(s): " + linha2,
                                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 2 Observação
                                    tf.DrawString(linha3 + ".",
                                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 51, 398, 14), format);//Linha 3 Observação
                                }
                            }


                        }
                        else
                        {
                            string Obs1 = pedido.Observacoes.Substring(0, 42);
                            string Obs2 = pedido.Observacoes.Substring(Obs1.Length, pedido.Observacoes.Length - Obs1.Length);

                            tf.DrawString(Obs1,
                            fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 24, 398, 14), format);//Linha 1 Observação

                            tf.DrawString(Obs2,
                            fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 1 Observação

                            if (linhaDatas.Length > 0)
                            {
                                if (linhaDatas.Length < 25)
                                {
                                    tf.DrawString("Referente(s) a(s) data(s): " + linhaDatas + ".",
                                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 52, 398, 14), format);//Linha 2 Observação
                                }
                                else
                                {
                                    string linha2 = linhaDatas.Substring(0, 24);
                                    string linha3 = linhaDatas.Substring(linha2.Length, linhaDatas.Length - linha2.Length);
                                    tf.DrawString("Referente(s) a(s) data(s): " + linha2,
                                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 52, 398, 14), format);//Linha 2 Observação
                                    tf.DrawString(linha3 + ".",
                                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 66, 398, 14), format);//Linha 3 Observação
                                }
                            }
                        }
                    }
                    else
                    {
                        if (linhaDatas.Length > 0)
                        {
                            if (linhaDatas.Length < 25)
                            {
                                tf.DrawString("Referente(s) a(s) data(s): " + linhaDatas + ".",
                                fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 2 Observação
                            }
                            else
                            {
                                string linha2 = linhaDatas.Substring(0, 24);
                                string linha3 = linhaDatas.Substring(linha2.Length, linhaDatas.Length - linha2.Length);
                                tf.DrawString("Referente(s) a(s) data(s): " + linha2,
                                fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 2 Observação
                                tf.DrawString(linha3 + ".",
                                fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 51, 398, 14), format);//Linha 3 Observação
                            }
                        }
                    }

                    //Escrito Créditos
                    graph.DrawRectangle(rect_style1, 318, RetRosaTab + 2, 105, 19);//Retangulo branco meio 1 linha
                    tf.DrawString("Créditos", fontTabelaNegrito, XBrushes.Black, new XRect(372, RetRosaTab + 4, 45, 18), format);

                    //Valor Crédito
                    string credito = pedido.Credito.ToString() + ",00";
                    graph.DrawRectangle(rect_style1, 425, RetRosaTab + 2, 142, 19);//Retangulo branco fim 1 linha
                    if (credito.TrimStart() != "0,00")
                    {
                        tf.DrawString("R$", fontParagraph, XBrushes.Black, new XRect(425, RetRosaTab + 4, 200, 18), format);

                        if (credito.Length > 6)
                        {
                            tf.DrawString(credito, fontParagraph, XBrushes.Black, new XRect(524, RetRosaTab + 4, 200, 18), format);
                        }
                        else
                        {
                            tf.DrawString(credito, fontParagraph, XBrushes.Black, new XRect(530, RetRosaTab + 4, 200, 18), format);
                        }
                             
                    }
                    //Escrito Descontos
                    graph.DrawRectangle(rect_style1, 318, RetRosaTab + 23, 105, 20);//Retangulo branco meio 2 linha
                    tf.DrawString("Descontos", fontTabelaNegrito, XBrushes.Black, new XRect(359, RetRosaTab + 25, 48, 18), format);

                    //Valor Descontos
                    string desconto = pedido.Desconto.ToString() + ",00";
                    graph.DrawRectangle(rect_style1, 425, RetRosaTab + 23, 142, 20);//Retangulo branco fim 2 linha
                    if (desconto.TrimStart() != "0,00") 
                    {
                        tf.DrawString("R$", fontParagraph, XBrushes.Black, new XRect(425, RetRosaTab + 25, 200, 18), format);
                        if (desconto.Length > 6)
                        {
                            tf.DrawString(desconto, fontParagraph, XBrushes.Black, new XRect(524, RetRosaTab + 25, 200, 18), format);
                        }
                        else
                        {
                            tf.DrawString(desconto, fontParagraph, XBrushes.Black, new XRect(530, RetRosaTab + 25, 200, 18), format);
                        }

                            

                    }
                    //Escrito Total a Pagar
                    graph.DrawRectangle(rect_style1, 318, RetRosaTab + 45, 105, 19);//Retangulo branco meio 3 linha
                    tf.DrawString("Total à Pagar", fontTabelaNegrito, XBrushes.Black, new XRect(344, RetRosaTab + 47, 100, 18), format);

                    SomaTotal = SomaSubtotal + pedido.Desconto - pedido.Credito;
                    string total = SomaTotal.ToString() + ",00";
                    //Valor Total a Pagar
                    graph.DrawRectangle(rect_style1, 425, RetRosaTab + 45, 142, 19);//Retangulo branco fim 3 linha
                    tf.DrawString("R$", fontCabecalho, XBrushes.Black, new XRect(425, RetRosaTab + 47, 200, 18), format);

                    if (total.Length > 6)
                    {
                        tf.DrawString(total, fontCabecalho, XBrushes.Black, new XRect(509, RetRosaTab + 47, 200, 18), format);
                    }
                    else
                    {
                        tf.DrawString(total, fontCabecalho, XBrushes.Black, new XRect(517, RetRosaTab + 47, 200, 18), format);
                    }
                    //Retangulo Atenção
                    graph.DrawRectangle(rect_style2, 20, RetRosaTab + 64, pdfPage.Width - 2 * 20, 84);
                    graph.DrawRectangle(rect_style1, 24, RetRosaTab + 66, pdfPage.Width - 2 * 20 - 7, 79);

                    tf.DrawString("Atenção!", fontTabelaNegrito, XBrushes.Black, new XRect(260, RetRosaTab + 68, 100, 18), format);
                    tf.DrawString("1- Este pedido não é valido como recebo de pagamento. O recibo será emitido após o pagamento",
                        fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 88, 570, 14), format);//Linha 1 Atenção
                    tf.DrawString("do boleto bancário referente aos atendimentos que constam neste pedido.",
                        fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 102, 570, 14), format);//Linha 1 Atenção
                    tf.DrawString("2-  Atendimentos não realizados por motivos de falta com justificativas prévia, poderão ser ",
                        fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 116, 570, 14), format);//Linha 1 Atenção
                    tf.DrawString("reagendados ou o(s) valor (es) descontado(s) no mês subsequente.",
                        fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 130, 570, 14), format);//Linha 1 Atenção

                }
            }

            return document;

        }


        public async Task<string> RetornaNomeMes(string mesRef)
        {

            return mesRef;
        }

    }
}
