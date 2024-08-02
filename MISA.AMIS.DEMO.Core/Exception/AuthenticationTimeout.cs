using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class AuthenticationTimeout : Exception
    {
        public AuthenticationTimeout() { }

        public AuthenticationTimeout(string message) : base(message) { }

        public string? DevMessage { get; set; }

        public string? UserMessage { get; set; }
    }
}
