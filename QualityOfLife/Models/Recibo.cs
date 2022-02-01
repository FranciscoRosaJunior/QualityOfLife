using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Recibo : Entidades
    {
        public Responsavel responsavel { get; set; }
        public Pedido pedido { get; set; }
    }
}
