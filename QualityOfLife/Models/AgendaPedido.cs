using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class AgendaPedido
    {
        [Key]
        public long Id { get; set; }
        public Pedido Pedido { get; set; }
        public Agenda Agenda { get; set; }
    }
}
