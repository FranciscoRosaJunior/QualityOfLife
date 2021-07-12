using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class ProfissionalPedido
    {
        [Key]
        public long Id { get; set; }
        public Paciente Pedido { get; set; }
        public Profissional Profissional { get; set; }
    }
}
