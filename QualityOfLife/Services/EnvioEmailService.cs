using Microsoft.Extensions.Options;
using QualityOfLife.Interfaces;
using QualityOfLife.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QualityOfLife.Services
{
    public class EnvioEmailService : IEnvioEmailService
    {
        public EnvioEmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }

        public Task EnviarEmailAsync(string email, string subject, string message, Byte[] arquivo, string nomeArquivo)
        {
            try
            {
                Execute(email, subject, message, arquivo, nomeArquivo).Wait();
                return Task.FromResult(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Execute(string email, string subject, string message, Byte[] arquivo, string nomeArquivo)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Marcella Rosa & Equipe")
                };

                mail.To.Add(new MailAddress(toEmail));
                mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.Subject = nomeArquivo + " - " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                if (!String.IsNullOrEmpty(nomeArquivo))
                {
                    //Attachment att = new Attachment(new MemoryStream(bytes), name);
                    Attachment anexar = new Attachment(new MemoryStream(arquivo), nomeArquivo);
                    mail.Attachments.Add(anexar);
                }

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
