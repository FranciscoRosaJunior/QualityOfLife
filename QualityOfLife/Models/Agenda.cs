using QualityOfLife.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Agenda : Entidades
    {
        public Paciente Paciente { get; set; }
        public Profissional Profissional { get; set; }
        public DateTime DataHora { get; set; }
        public string Local { get; set; }
        public TipoAtendimento TipoAtendimento { get; set; }
        public LocalAtendimento LocalAtendimento { get; set; }
        public bool Presenca { get; set; }
        public bool FaltaJustificada { get; set; }
        public bool Falta { get; set; }
        public bool Reagendar { get; set; }
        public string Anotações { get; set; }
        public int Repetir { get; set; }
        public string Valor { get; set; }



        public Agenda()
        {
        }

        public Agenda(Paciente paciente, Profissional profissional, DateTime dataHora, string local, TipoAtendimento tipoAtendimento, bool presenca, bool faltaJustificada, bool falta, bool reagendar, string anotações, int repetir, string valor)
        {
            Paciente = paciente;
            Profissional = profissional;
            DataHora = dataHora;
            Local = local;
            TipoAtendimento = tipoAtendimento;
            Presenca = presenca;
            FaltaJustificada = faltaJustificada;
            Falta = falta;
            Reagendar = reagendar;
            Anotações = anotações;
            Repetir = repetir;
            Valor = valor;
        }
    }
}
