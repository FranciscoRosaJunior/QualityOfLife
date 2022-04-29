using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using QualityOfLife.Models.Enums;

namespace QualityOfLife.Models
{
    public class AgendaEmLote
    {
        public Profissional Profissional { get; set; }
        [DisplayName("Mês Referência")]
        public DateTime MesReferencia { get; set; }
        public string Local { get; set; }
        [DisplayName("Tipo de Atendimento")]
        public TipoAtendimento TipoAtendimento { get; set; }
        public string Valor { get; set; }

        public AgendaEmLote()
        {
        }

        public AgendaEmLote(Profissional profissional, DateTime mesReferencia, string local, TipoAtendimento tipoAtendimento, string valor)
        {
            Profissional = profissional;
            MesReferencia = mesReferencia;
            Local = local;
            TipoAtendimento = tipoAtendimento;
            Valor = valor;
        }
    }
}
