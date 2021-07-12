using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using QualityOfLife.Data;
using QualityOfLife.Models;

namespace QualityOfLife.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pedido
                .Include(x => x.Paciente)
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public async Task<IActionResult> Create(string paciente, string mes)
        {
            ViewBag.cpf = paciente;
            ViewBag.Mes = mes;
            ViewBag.Paciente = await _context.Paciente.ToListAsync();
            if (paciente == null)
            {
                List<Agenda> model = new List<Agenda>();
                return View(model);
            }
            else
            {
                var model = await _context.Agenda.Where(x => x.Paciente.Cpf == paciente)
                    .Where(x => x.DataHora.ToString()
                    .Contains(mes)).Include(x => x.Profissional).ToListAsync();
                return View(model);
            }

        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TipoAtendimento,DataPedido,Valor,Desconto,Credito,Total,Observações,Id,Criado,CriadoData,Modificado,ModificadoData")] Pedido pedido)
        public async Task<IActionResult> CreatePost(Pedido pedido, string pacientePost, string mesPost)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Agenda.Where(x => x.Paciente.Cpf == pacientePost)
                    .Where(x => x.DataHora.ToString().Contains(mesPost))
                    .Include(x => x.Profissional)
                    .Include(x => x.Paciente)
                    .ToListAsync();

                pedido.CriadoData = DateTime.Now;
                pedido.Criado = User.Identity.Name;
                pedido.DataPedido = DateTime.Now;
                pedido.Valor = 1200.00;
                pedido.Credito = 320.00;
                pedido.Desconto = 100.00;
                pedido.Total = 1200.00 - 320.00 + 100.00;
                pedido.Paciente = model.Select(x => x.Paciente).FirstOrDefault();

                //chama função que gera o pedido em pdf
                //passando o pedido e o model da agenda

                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //GerarPdf
        public async Task<FileResult> GerarPdf(long? id)
        {
            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                var graphics = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                var corRosa = PdfSharpCore.Drawing.XBrushes.Magenta;
                var corPreto = PdfSharpCore.Drawing.XBrushes.Black;

                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                var pedido = new XFont("Arial", 26, XFontStyle.Bold);
                var dataEnumero = new XFont("Arial", 12, XFontStyle.Bold);
                var fonteColuna = new PdfSharpCore.Drawing.XFont("Arial", 10, PdfSharpCore.Drawing.XFontStyle.BoldItalic);



                var fonteOrganzacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                var fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                var titulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                var fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 7);

                var logo = @"D:\Francisco\Projetos Gerais\QualityOfLife\QualityOfLife\wwwroot\images\imagempedido.png";

                var qtdPaginas = doc.PageCount;
                textFormatter.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corPreto, new XRect(578, 825, page.Width, page.Height));

                // Impressão do LogoTipo
                XImage imagem = XImage.FromFile(logo);
                graphics.DrawImage(imagem, 10, 10, 570, 200);


                //// Titulo Pedido
                //textFormatter.DrawString("Pedido", pedido, corRosa, new PdfSharpCore.Drawing.XRect(50, 210, page.Width, page.Height));
                ////Data
                //textFormatter.DrawString("Data:", dataEnumero, corPreto, new PdfSharpCore.Drawing.XRect(70, 210, page.Width, page.Height));

                //textFormatter.DrawString("Número:", dataEnumero, corPreto, new PdfSharpCore.Drawing.XRect(70, 220, page.Width, page.Height));


                // titulo das colunas
                var alturaTituloDetalhesY = 250;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                detalhes.DrawString("Data", fonteColuna, corPreto, new PdfSharpCore.Drawing.XRect(20, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Tipo", fonteColuna, corPreto, new PdfSharpCore.Drawing.XRect(144, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Profissional", fonteColuna, corPreto, new PdfSharpCore.Drawing.XRect(220, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Valor", fonteColuna, corPreto, new PdfSharpCore.Drawing.XRect(290, alturaTituloDetalhesY, page.Width, page.Height));

                ////dados do relatório 
                //var alturaDetalhesItens = 160;
                //for (int i = 1; i < 30; i++)
                //{

                //    textFormatter.DrawString("Descrição" + " : " + i.ToString(), fonteDetalhesDescricao, corPreto, new PdfSharpCore.Drawing.XRect(21, alturaDetalhesItens, page.Width, page.Height));
                //    textFormatter.DrawString("Atendimento" + " : " + i.ToString(), fonteDetalhesDescricao, corPreto, new PdfSharpCore.Drawing.XRect(145, alturaDetalhesItens, page.Width, page.Height));
                //    textFormatter.DrawString("Operação" + " : " + i.ToString(), fonteDetalhesDescricao, corPreto, new PdfSharpCore.Drawing.XRect(215, alturaDetalhesItens, page.Width, page.Height));
                //    textFormatter.DrawString("Quantidade" + " : " + i.ToString(), fonteDetalhesDescricao, corPreto, new PdfSharpCore.Drawing.XRect(290, alturaDetalhesItens, page.Width, page.Height));
                //    textFormatter.DrawString("Status" + " : " + i.ToString(), fonteDetalhesDescricao, corPreto, new PdfSharpCore.Drawing.XRect(332, alturaDetalhesItens, page.Width, page.Height));
                //    textFormatter.DrawString(DateTime.Now.ToString(), fonteDetalhesDescricao, corPreto, new PdfSharpCore.Drawing.XRect(400, alturaDetalhesItens, page.Width, page.Height));

                //    alturaDetalhesItens += 20;
                //}


                using (MemoryStream stream = new MemoryStream())
                {
                    var contantType = "application/pdf";
                    doc.Save(stream, false);

                    var nomeArquivo = "RelatorioValdir.pdf";

                    return File(stream.ToArray(), contantType, nomeArquivo);
                }

            }
        }

        public async Task<FileResult> GerarPdf2(long? id)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Table Example";

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


                // page structure options
                double lineHeight = 20;
                int rect_height = 17;

                int borda = 23;
                int bordaRetangulo = 3;
                int RetRosaTab = 372;
                int RetBrancoTab = 374;
                string observacoes = "Este teste é para saber se a string possui mais de (42) quarenta e dois caracteres. Este teste é para saber se a";

                XSolidBrush rect_style1 = new XSolidBrush(XColors.White);
                XSolidBrush rect_style2 = new XSolidBrush(XColors.Magenta) ;

                var logo = @"D:\Francisco\Projetos Gerais\QualityOfLife\QualityOfLife\wwwroot\images\imagempedido.png";

                



                for (int i = 0; i < 10; i++)
                {
                    double dist_Y = lineHeight * (i + 1);
                    double dist_Y2 = dist_Y - 2;

                    // header della G
                    if (i == 0)
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
                        tf.DrawString("08/07/2021", fontParagraph, XBrushes.Black, new XRect(430, 225, 980, 30), format);

                        //Escrito Número
                        tf.DrawString("Número:", fontParagraph, XBrushes.Black, new XRect(376, 253, 980, 30), format);
                        graph.DrawRectangle(rect_style2, 425, 246, 145, 22);//Retangulo Rosa
                        graph.DrawRectangle(rect_style1, 427, 248, 141, 18);//Retangulo Branco
                        tf.DrawString("2021/0001", fontParagraph, XBrushes.Black, new XRect(430, 251, 980, 30), format);

                        //Cabeçalho de dados do cliente
                        graph.DrawRectangle(rect_style2, 20, 272, pdfPage.Width - 2 * 20, 80);
                        tf.DrawString("Dados do Cliente", fontCabecalho, XBrushes.White, new XRect(230, 273, 145, 17), format);

                        // Informações Cabeçalho de dados do cliente
                        graph.DrawRectangle(rect_style1, 24, 289, pdfPage.Width - 2 * 20 - 7, 60);
                        tf.DrawString("Responsável: Francisco Bonitão Junior", fontParagraph, XBrushes.Black, new XRect(26, 291, 145, 17), format);
                        tf.DrawString("Criança: Lucas e Bernardo Bonitões", fontParagraph, XBrushes.Black, new XRect(26, 305, 145, 17), format);
                        tf.DrawString("Contato: (31) 99999 - 6666", fontParagraph, XBrushes.Black, new XRect(26, 319, 145, 17),format);
                        tf.DrawString("E mail: chicolindao @gmail.com", fontParagraph, XBrushes.Black, new XRect(26, 333, 145, 17), format);

                        //Retangulo Rosa Cabeçalho tabela
                        graph.DrawRectangle(rect_style2, 20, 355, pdfPage.Width - 2 * 20, 17);

                        tf.DrawString("Tabela de Atendimentos", fontParagraph, XBrushes.White,
                                      new XRect(150, 357, 80, 30), format);

                        //tf.DrawString("Tipo", fontParagraph, XBrushes.White,
                        //              new XRect(135, 357, 380, 30), format);

                        //tf.DrawString("Profissional", fontParagraph, XBrushes.White,
                        //              new XRect(260, 357, 680, 30), format);

                        //tf.DrawString("Valor", fontParagraph, XBrushes.White,
                        //              new XRect(435, 357, 980, 30), format);//372

                        graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 22);//387

                        //Primeira Linha e Primeira coluna
                        graph.DrawRectangle(rect_style1, 20 + bordaRetangulo, RetBrancoTab, 125 - borda - bordaRetangulo, rect_height);
                        tf.DrawString("Data", fontParagraph, XBrushes.Black, new XRect(25, RetBrancoTab, 80, 30), format);

                        ////ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style1, 125, RetBrancoTab, 146 - borda, rect_height);
                        tf.DrawString("Tipo", fontParagraph, XBrushes.Black, new XRect(127, RetBrancoTab, 380, 30), format);

                        ////ELEMENT 3 - SMALL 80
                        graph.DrawRectangle(rect_style1, 250, RetBrancoTab, 195 - borda, rect_height);
                        tf.DrawString("Profissional", fontParagraph, XBrushes.Black, new XRect(252, RetBrancoTab, 680, 30), format);

                        ////ELEMENT 4 - SMALL 80
                        graph.DrawRectangle(rect_style1, 425, RetBrancoTab, 145 - bordaRetangulo, rect_height);
                        tf.DrawString("Valor", fontParagraph, XBrushes.Black, new XRect(427, RetBrancoTab, 980, 30), format);
                    }
                    else
                    {
                        RetRosaTab = RetRosaTab + 17;
                        RetBrancoTab = RetBrancoTab + 17;

                        graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 22);//387

                        //Primeira Linha e Primeira coluna
                        graph.DrawRectangle(rect_style1, 20 + bordaRetangulo, RetBrancoTab, 125 - borda - bordaRetangulo, rect_height);
                        tf.DrawString("07/07/2021", fontParagraph, XBrushes.Black, new XRect(25, RetBrancoTab, 80, 30), format);

                        ////ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style1, 125, RetBrancoTab, 146 - borda, rect_height);
                        tf.DrawString("Consultório", fontParagraph, XBrushes.Black, new XRect(127, RetBrancoTab, 380, 30), format);

                        ////ELEMENT 3 - SMALL 80
                        graph.DrawRectangle(rect_style1, 250, RetBrancoTab, 195 - borda, rect_height);
                        tf.DrawString("Marcella TO", fontParagraph, XBrushes.Black, new XRect(252, RetBrancoTab, 680, 30), format);

                        ////ELEMENT 4 - SMALL 80
                        graph.DrawRectangle(rect_style1, 425, RetBrancoTab, 145 - bordaRetangulo, rect_height);
                        tf.DrawString("R$ 160,00", fontParagraph, XBrushes.Black, new XRect(427, RetBrancoTab, 980, 30), format);

                    }
                }
                RetRosaTab = RetRosaTab + 25;
                //
                graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 22);
                graph.DrawRectangle(rect_style1, 24, RetRosaTab + 2, 398, 18);
                tf.DrawString("SUBTOTAL", fontTabelaNegrito, XBrushes.Black, new XRect(26, RetRosaTab + 4, 980, 30), format);
                graph.DrawRectangle(rect_style1, 425, RetRosaTab + 2, 142, 18);
                tf.DrawString("R$ 800,00", fontParagraph, XBrushes.Black, new XRect(427, RetRosaTab + 4, 980, 30), format);

                RetRosaTab = RetRosaTab + 25;
                graph.DrawRectangle(rect_style2, 20, RetRosaTab, pdfPage.Width - 2 * 20, 66);//Retangulo Rosa Maior
                graph.DrawRectangle(rect_style1, 24, RetRosaTab + 2, 291, 61);//Retangulo Branco Maior
                tf.DrawString("Observações", fontTabelaNegrito, XBrushes.Black, new XRect(130, RetRosaTab + 4, 200, 18), format);

                if(observacoes.Length <= 50)
                {
                    tf.DrawString(observacoes,//max 42 caracteres
                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 24, 398, 14), format);//Linha 1 Observação
                }
                else if (observacoes.Length > 50 && observacoes.Length <= 100)
                {
                    string linha1 = observacoes.Substring(0, 50);
                    string linha2 = observacoes.Substring(linha1.Length, observacoes.Length - linha1.Length);
                    tf.DrawString(linha1,
                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 24, 398, 14), format);//Linha 1 Observação
                    tf.DrawString(linha2.TrimStart(),
                        fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 2 Observação
                }
                else if (observacoes.Length > 100 && observacoes.Length <= 150)
                {
                    string linha1 = observacoes.Substring(0, 50);
                    string linha2 = observacoes.Substring(linha1.Length, 50);
                    string linha3 = observacoes.Substring(linha2.Length, observacoes.Length - linha2.Length);
                    tf.DrawString(linha1,
                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 24, 398, 14), format);//Linha 1 Observação
                    tf.DrawString(linha2.TrimStart(),
                        fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 2 Observação
                    tf.DrawString(linha3.TrimStart(),
                    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 51, 398, 14), format);//Linha 3 Observação

                }

                //tf.DrawString("0123456789 0123456789 0123456789 012345678",//max 42 caracteres
                //    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 24, 398, 14), format);//Linha 1 Observação
                //tf.DrawString("0123456789 0123456789 0123456789 012345678",//max 42 caracteres
                //    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 38, 398, 14), format);//Linha 2 Observação
                //tf.DrawString("0123456789 0123456789 0123456789 012345678",//max 42 caracteres
                //    fontParagraph, XBrushes.Black, new XRect(26, RetRosaTab + 51, 398, 14), format);//Linha 3 Observação

                //Escrito Créditos
                graph.DrawRectangle(rect_style1, 318, RetRosaTab + 2, 105, 19);//Retangulo branco meio 1 linha
                tf.DrawString("Créditos", fontTabelaNegrito, XBrushes.Black, new XRect(372, RetRosaTab + 4, 45, 18), format);

                //Valor Crédito
                graph.DrawRectangle(rect_style1, 425, RetRosaTab + 2, 142, 19);//Retangulo branco fim 1 linha
                tf.DrawString("R$ 320,00", fontParagraph, XBrushes.Black, new XRect(427, RetRosaTab + 4, 142, 18), format);

                //Escrito Descontos
                graph.DrawRectangle(rect_style1, 318, RetRosaTab + 23, 105, 20);//Retangulo branco meio 2 linha
                tf.DrawString("Descontos", fontTabelaNegrito, XBrushes.Black, new XRect(359, RetRosaTab + 25, 48, 18), format);

                //Valor Descontos
                graph.DrawRectangle(rect_style1, 425, RetRosaTab + 23, 142, 20);//Retangulo branco fim 2 linha
                tf.DrawString("R$ 100,00", fontParagraph, XBrushes.Black, new XRect(427, RetRosaTab + 25, 142, 18), format);

                //Escrito Total a Pagar
                graph.DrawRectangle(rect_style1, 318, RetRosaTab + 45, 105, 19);//Retangulo branco meio 3 linha
                tf.DrawString("Total à Pagar", fontTabelaNegrito, XBrushes.Black, new XRect(344, RetRosaTab + 47, 100, 18), format);

                //Valor Total a Pagar
                graph.DrawRectangle(rect_style1, 425, RetRosaTab + 45, 142, 19);//Retangulo branco fim 3 linha
                tf.DrawString("R$ 580,00", fontParagraph, XBrushes.Black, new XRect(427, RetRosaTab + 47, 142, 18), format);

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



            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                var contantType = "application/pdf";
                document.Save(stream, false);

                var nomeArquivo = "Pedido.pdf";

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
        public async Task<IActionResult> Edit(long id, [Bind("TipoAtendimento,DataPedido,Valor,Desconto,Credito,Total,Observações,Id,Criado,CriadoData,Modificado,ModificadoData")] Pedido pedido)
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
