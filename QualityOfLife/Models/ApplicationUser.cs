using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? Registered { get; set; } = DateTime.Now;
        public List<RegisteredLogins> RegisteredLogin { get; set; } = new List<RegisteredLogins>();
    }
}
