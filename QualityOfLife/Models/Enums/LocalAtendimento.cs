using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models.Enums
{
    public enum LocalAtendimento
    {
        [Description("Sala TO-1")]
        Sala_TO1,
        [Description("Sala TO-2")]
        Sala_TO2,
        [Description("Sala Fono-1")]
        Sala_Fono1,
        [Description("Sala Fono-2")]
        Sala_Fono2,
        [Description("Sala Psico-1")]
        Sala_Psico1,
        [Description("Sala Psico-2")]
        Sala_Psico2
    }
}
