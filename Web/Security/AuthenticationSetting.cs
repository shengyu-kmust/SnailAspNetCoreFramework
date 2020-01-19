using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Security
{
    public class AuthenticationSetting
    {
        public string AccessDeniedPath { get; set; }
        public string LoginPath { get; set; }
        public TimeSpan ExpireTimeSpan { get; set; }
        public string RsaPublicKey { get; set; }
        public string RsaPrivateKey { get; set; }
        public string SymmetricSecurityKey { get; set; }
        public bool IsAsymmetric { get; set; }

    }
}
