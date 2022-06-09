using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class RegisteredLogins
    {
        [Key]
        public int Id { get; set; }
        public string Provider { get; set; }
        public bool Action { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
