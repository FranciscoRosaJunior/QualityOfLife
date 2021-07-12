using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Boleto : Entidades
    {
        public string Banco { get; set; } = "Banco Inter";
        public string Numero { get; set; }
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public Responsavel PacienteResponsavel { get; set; }

        public Boleto()
        {
        }

        public Boleto(string banco, string numero, DateTime dataVencimento, double valor, Responsavel pacienteResponsavel, int pacienteResponsavelId)
        {
            Banco = banco;
            Numero = numero;
            DataVencimento = dataVencimento;
            Valor = valor;
            PacienteResponsavel = pacienteResponsavel;
        }
    }
}
