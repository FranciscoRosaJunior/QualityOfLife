using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Interfaces
{
    public interface IEnvioEmailService
    {
        Task EnviarEmailAsync(string email, string subject, string message, Byte[] arquivo, string nomeArquivo);
    }
}
