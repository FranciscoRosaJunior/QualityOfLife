using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models.ViewModels
{
    public class CalendarioViewModels
    {
        public DateTime DataHora { get; set; }
        public string Anotações { get; set; }
        public string NomePaciente { get; set; }
        public string NomeProfissional { get; set; }

        //public Agenda agendas { get; set; }
        //public Paciente pacientes { get; set; }
        //public Profissional profissionais { get; set; }
    }
}
