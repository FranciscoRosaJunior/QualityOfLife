using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class ProfissionalPaciente
    {
        [Key]
        public long Id { get; set; }
        public Paciente Paciente { get; set; }
        public Profissional Profissional { get; set; }
    }
}
