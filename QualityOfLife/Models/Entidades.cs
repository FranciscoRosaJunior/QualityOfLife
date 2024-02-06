using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Entidades
    {
        [Key]
        public long Id { get; set; }

        [DisplayName("CRIADOR")]
        public string Criado { get; set; }

        [DisplayName("DATA DE CRIAÇÃO")]
        [Column(TypeName = "DATETIME")]
        public DateTime CriadoData { get; set; }

        [DisplayName("MODIFICADOR")]
        public string Modificado { get; set; }

        [DisplayName("DATA DE MODIFICAÇÃO")]
        [Column(TypeName = "DATETIME")]
        public DateTime ModificadoData { get; set; }

        


    }
}
