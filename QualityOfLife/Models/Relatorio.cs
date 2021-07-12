using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Relatorio : Entidades
    {
        public DateTime DataRelatorio { get; set; }
        public string Status { get; set; }
        public string Link { get; set; }
        public Paciente Paciente { get; set; }
        public Profissional Profissional { get; set; }

        public Relatorio()
        {
        }

        public Relatorio(DateTime dataRelatorio, string status, string link, Paciente paciente, int pacienteId, Profissional profissional, int profissionalId)
        {
            DataRelatorio = dataRelatorio;
            Status = status;
            Link = link;
            Paciente = paciente;
            Profissional = profissional;
        }
    }
}
