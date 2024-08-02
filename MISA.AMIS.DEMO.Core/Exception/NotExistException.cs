using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class NotExistException : Exception
    {
        public NotExistException() : base() { }

        public NotExistException(string message) : base(message) { }

        public NotExistException(string userMessage, object errors)
        {
            Errors = errors;
            UserMessage = userMessage;
        }

        public string? UserMessage { get; set; }

        public string? DevMessage { get; set; }

        public object? Errors { get; set; }
    }
}
