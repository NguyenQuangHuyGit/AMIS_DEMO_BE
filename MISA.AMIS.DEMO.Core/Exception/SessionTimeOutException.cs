using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class SessionTimeOutException : Exception
    {
        public SessionTimeOutException() : base() { }

        public SessionTimeOutException(string message) : base(message) { }

        public string? UserMessage { get; set; }

        public string? DevMessage { get; set; }
    }
}
