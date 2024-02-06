using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models.Enums
{
    public enum TipoContaPagar
    {
        [Description("Consultório Fixa")]
        Consultorio_Fixa,
        [Description("Consultório Variável")]
        Consultório_Variavel,
        [Description("Casa Fixa")]
        Casa_Fixa,
        [Description("Casa Variável")]
        Casa_Variavel
            
    }
}
