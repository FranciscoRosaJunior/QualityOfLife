using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class EmailSettings
    {
        public String PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public String UsernameEmail { get; set; }
        public String UsernamePassword { get; set; }
        public String FromEmail { get; set; }
        public String ToEmail { get; set; }
        public String CcEmail { get; set; }

        public EmailSettings()
        {
        }

        public EmailSettings(string primaryDomain, int primaryPort, string usernameEmail, string usernamePassword, string fromEmail, string toEmail, string ccEmail)
        {
            PrimaryDomain = primaryDomain;
            PrimaryPort = primaryPort;
            UsernameEmail = usernameEmail;
            UsernamePassword = usernamePassword;
            FromEmail = fromEmail;
            ToEmail = toEmail;
            CcEmail = ccEmail;
        }
    }
}
