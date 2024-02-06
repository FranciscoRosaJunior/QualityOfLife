using QualityOfLife.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class PacienteDiaAtendimento
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Paciente")]
        public Paciente Paciente { get; set; }
        [DisplayName("Dia da Semana")]
        public string DiaDaSemana{ get; set; }
        [DisplayName("Horário")]
        public DateTime Horario { get; set; }

        public PacienteDiaAtendimento()
        {
        }

        public PacienteDiaAtendimento(long id, Paciente paciente, string diaDaSemana, DateTime horario)
        {
            Id = id;
            Paciente = paciente;
            DiaDaSemana = diaDaSemana;
            Horario = horario;
        }
    }
}
