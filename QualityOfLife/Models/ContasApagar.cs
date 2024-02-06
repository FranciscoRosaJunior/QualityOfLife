using QualityOfLife.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class ContasApagar : Entidades
    {
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
        [DisplayName("Tipo")]
        public TipoContaPagar TipoContaPagar { get; set; }

        public double Valor { get; set; }

        [DisplayName("Vencimento")]
        public DateTime DataVencimento { get; set; }

        [DisplayName("Pagamento Realizado?")]
        public bool Pagamento { get; set; }

        [DisplayName("Data Pag.")]
        public DateTime? DataPagamento { get; set; }

        [DisplayName("Forma Pagamento")]
        public int FormaPagamento { get; set; }

        [DisplayName("Quantidade de Parcelas")]
        public int QuantParc { get; set; }

        [DisplayName("Observações:")]
        public string Observacoes { get; set; }

        public ContasApagar()
        {
        }

        public ContasApagar(string descricao, double valor, DateTime dataVencimento, bool pagamento, DateTime? dataPagamento, string observacoes)
        {
            Descricao = descricao;
            Valor = valor;
            DataVencimento = dataVencimento;
            Pagamento = pagamento;
            DataPagamento = dataPagamento;
            Observacoes = observacoes;
        }
    }
}
