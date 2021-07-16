using ConsultorioTO.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Pedido : Entidades
    {
        public TipoAtendimento TipoAtendimento { get; set; }
        [DisplayName("Data do Pedido")]
        public DateTime DataPedido { get; set; }
        [DisplayName("Mês Referência")]
        public string mesreferencia { get; set; }
        public double Valor { get; set; }
        public double Desconto { get; set; }
        [DisplayName("Crédito")]
        public double Credito { get; set; }
        public double Total { get; set; }
        public Paciente Paciente { get; set; }
        public Profissional Profissional { get; set; }
        public string Observacoes { get; set; }

        public Pedido()
        {
        }

        public Pedido(TipoAtendimento tipoAtendimento, DateTime dataPedido, string mesreferencia, double valor, double desconto, double credito, double total, Paciente paciente, Profissional profissional, string observacoes)
        {
            TipoAtendimento = tipoAtendimento;
            DataPedido = dataPedido;
            this.mesreferencia = mesreferencia;
            Valor = valor;
            Desconto = desconto;
            Credito = credito;
            Total = total;
            Paciente = paciente;
            Profissional = profissional;
            Observacoes = observacoes;
        }
    }
}
