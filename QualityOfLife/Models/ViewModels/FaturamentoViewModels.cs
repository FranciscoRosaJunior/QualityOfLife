using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models.ViewModels
{
    public class FaturamentoViewModels
    {
        public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
        public ICollection<Agenda> Agendas { get; set; } = new List<Agenda>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
