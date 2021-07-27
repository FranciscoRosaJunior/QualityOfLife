using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models.Enums
{
    public enum FormaPagamento : int
    {
        Dinheiro = 1,
        Boleto = 2,
        Pix = 3,
        Transferência = 4,
        Debito = 5,
        Crédito = 6
    }
}
