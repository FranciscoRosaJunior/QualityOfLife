using QualityOfLife.Models.Enums;
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
        [DisplayName("Valor dos Atendimentos")]
        public double Valor { get; set; }
        public double Desconto { get; set; }
        [DisplayName("Crédito")]
        public double Credito { get; set; }
        public double Total { get; set; }
        public Paciente Paciente { get; set; }
        public Profissional Profissional { get; set; }
        [DisplayName("Observações:")]
        public string Observacoes { get; set; }
        [DisplayName("Pagamento Realizado")]
        public bool Pagamento { get; set; }
        [DisplayName("Data de Pagamento")]
        public DateTime DataPagamento { get; set; }
        [DisplayName("Forma Pagamento")]
        public int FormaPagamento { get; set; }
        [DisplayName("Local Pagamento")]
        public int LocalPagamento { get; set; }
        [DisplayName("Recibo Emitido?")]
        public bool ReciboEmitido { get; set; }
        //Pagamento, data Pagamento, Modo Pagamento, Local Pagamento, Recibo

        public Pedido()
        {
        }

        public Pedido(TipoAtendimento tipoAtendimento, DateTime dataPedido, string mesreferencia, double valor, double desconto, double credito, double total, Paciente paciente, Profissional profissional, string observacoes, bool pagamento, DateTime dataPagamento, int formaPagamento, int localPagamento, bool reciboEmitido)
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
            Pagamento = pagamento;
            DataPagamento = dataPagamento;
            FormaPagamento = formaPagamento;
            LocalPagamento = localPagamento;
            ReciboEmitido = reciboEmitido;
        }
    }
}
