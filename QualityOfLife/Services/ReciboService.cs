using PdfSharpCore.Pdf;
using QualityOfLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using QualityOfLife.Data;
using System.IO;

namespace QualityOfLife.Services
{
    public class ReciboService
    {
        public async Task<PdfDocument> PdfRecibo(Responsavel responsavel, Pedido pedido, List<AgendaPedido> agendas)
        {
            var logo = @"D:\Francisco\Projetos Gerais\QualityOfLife\QualityOfLife\wwwroot\images\imagemrecibo.png";
            var assinatura = @"D:\Francisco\Projetos Gerais\QualityOfLife\QualityOfLife\wwwroot\images\assinatura.png";
            var rodape = @"D:\Francisco\Projetos Gerais\QualityOfLife\QualityOfLife\wwwroot\images\papeltimbradorodape.png";
            var dadosEmpresa = @"D:\Francisco\Projetos Gerais\QualityOfLife\QualityOfLife\wwwroot\images\dadosempresa.png";


            //instancia document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Recibo " + responsavel.Nome + " - " + responsavel.Cpf + pedido.DataPedido.ToString("yyyyMMddHHmmss");

            for (int p = 0; p < 1; p++)
            {
                // Page Options
                PdfPage pdfPage = document.AddPage();
                pdfPage.Height = 842;//842
                pdfPage.Width = 590;
                var larguraPage = pdfPage.Width - 2 * 20;


                // Get an XGraphics object for drawing
                XGraphics graph = XGraphics.FromPdfPage(pdfPage);

                // Text format
                XStringFormat format = new XStringFormat();
                format.LineAlignment = XLineAlignment.Near;
                format.Alignment = XStringAlignment.Near;
                var tf = new XTextFormatter(graph);

                XFont fontParagraph = new XFont("Arial", 12, XFontStyle.Regular);
                
                //pontos da logo
                int xImagem = 180;
                int yImagem = 20;
                int alturaImagem = 60;
                int larguraImagem = 231;

                //pontos do titulo
                string titulo = "Recibo";
                int xTitulo = 270;
                int yTitulo = 170;
                XFont fonttitulo = new XFont("Arial", 20, XFontStyle.Bold);

                //pontos do valor
                string valor = "R$ " + pedido.Valor + ",00";
                int xValor = 60;
                int yValor = yTitulo + 56;
                string valorPorExtenso = toExtenso(Convert.ToDecimal(pedido.Valor));
                
                //Agenda datahora
                string contAgendaPorExtenso = toExtenso(agendas.Count());
                string mes = "";
                string ano = "";
                string linha1 = "Recebi de " + responsavel.Nome + ", portador(a) do CPF " + responsavel.Cpf + ",";
                string linha2 = "a importância de " + valorPorExtenso + ", referente a " + contAgendaPorExtenso.Replace("reais","") + "atendimento(s) de";
                string linha3 = "terapia ocupacional realizado a " + pedido.Paciente.Nome + " no(s) dia(s) ";
                string linha4 =  "";
                int TotalAgendas = 1;

                foreach (var agenda in agendas)
                {
                    if (TotalAgendas != agendas.Count())
                    {
                        linha4 = linha4 + agenda.Agenda.DataHora.Day.ToString() + ", ";
                        TotalAgendas++;
                    }
                    else
                    {
                        mes = agenda.Agenda.DataHora.ToString("MMMM");
                        ano = agenda.Agenda.DataHora.Year.ToString();
                        linha4 = linha4 + agenda.Agenda.DataHora.Day.ToString() + " de " + mes + " " + ano + ".";
                    }
                    
                }

                int xtexto = 60;
                int ytexto = yValor + 14;
                int tamanhoCaracter = 14;
                XFont fonttexto = new XFont("Arial", 20, XFontStyle.Bold);

                //Imagem do cabeçalho
                XImage imagem = XImage.FromFile(logo);
                graph.DrawImage(imagem, xImagem, yImagem, larguraImagem, alturaImagem);

                //Titulo
                tf.DrawString(titulo, fonttitulo, XBrushes.Black, new XRect(xTitulo, yTitulo, larguraPage, 24), format);

                //Valor
                tf.DrawString(valor, fontParagraph, XBrushes.Black, new XRect(xValor, yValor, 398, 14), format);

                //Texto
                tf.DrawString(linha1.TrimStart(), fontParagraph, XBrushes.Black, new XRect(xtexto, ytexto, larguraPage - 40, tamanhoCaracter), format);
                ytexto = ytexto + 14;                                                                                    
                tf.DrawString(linha2.TrimStart(), fontParagraph, XBrushes.Black, new XRect(xtexto, ytexto, larguraPage - 40, tamanhoCaracter), format);
                ytexto = ytexto + 14;                                                                                    
                tf.DrawString(linha3.TrimStart(), fontParagraph, XBrushes.Black, new XRect(xtexto, ytexto, larguraPage - 40, tamanhoCaracter), format);
                ytexto = ytexto + 14;                                                                                    
                tf.DrawString(linha4.TrimStart(), fontParagraph, XBrushes.Black, new XRect(xtexto, ytexto, larguraPage - 40, tamanhoCaracter), format);
                ytexto = ytexto + 14;

                //local e data
                string localData = "Belo Horizonte, " + DateTime.Now.ToString("dd") + " de " + DateTime.Now.ToString("MMMM") + " de " + DateTime.Now.ToString("yyyy") + ".";

                //assinatura
                int xassinatura = 185;
                int yassinatura = 360;
                int alturaassinatura = 115;
                int larguraassinatura = 240;
                XImage assinaturaImg = XImage.FromFile(assinatura);
                graph.DrawImage(assinaturaImg, xassinatura, yassinatura, larguraassinatura, alturaassinatura);

                //pontos do rodape
                int xrodape = 1;
                int yrodape = 742;
                int alturarodape = 98;
                int largurarodape = 588;

                XImage rodapeImg = XImage.FromFile(rodape);
                graph.DrawImage(rodapeImg, xrodape, yrodape, largurarodape, alturarodape);

                //dadosLocal
                int xdadosLocal = 165;
                int ydadosLocal = 728;
                int alturadadosLocal = 95;
                int larguradadosLocal = 280;

                XImage dadosLocalImg = XImage.FromFile(dadosEmpresa);
                graph.DrawImage(dadosLocalImg, xdadosLocal, ydadosLocal, larguradadosLocal, alturadadosLocal);


            }

            return document;

        }

        public static string toExtenso(decimal valor)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " DE";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                valor_por_extenso += " DE";
                            else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                valor_por_extenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valor_por_extenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valor_por_extenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTAVOS";
                }
                return valor_por_extenso.ToLower();
            }
        }

        static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }
    }
}
