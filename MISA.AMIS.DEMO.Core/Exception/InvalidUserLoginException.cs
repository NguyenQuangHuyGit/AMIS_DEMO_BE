using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class InvalidUserLoginException : Exception
    {
        public InvalidUserLoginException() : base() { }

        public InvalidUserLoginException(string message) : base(message) { }

        public string? UserMessage { get; set; }

        public string? DevMessage { get; set; }
    }
}
