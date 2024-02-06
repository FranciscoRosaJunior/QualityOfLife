using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models.ViewModels
{
    public class FaturamentoViewModels
    {
        public Paciente Paciente { get; set; } = new Paciente();
        public ICollection<Agenda> Agendas { get; set; } = new List<Agenda>();
        public Pedido Pedido { get; set; } = new Pedido();

        public EmailViewModel EmailViewModel { get; set; } = new EmailViewModel();
    }
}
