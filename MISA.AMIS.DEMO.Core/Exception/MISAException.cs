using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Custom Exception format
    /// </summary>
    public class MISAException
    {
        #region Fields
        public string? DevMessage { get; set; }
        public string? UserMessage { get; set; }
        public string? TraceId { get; set; }
        public string? MoreInfo { get; set; }
        public object? Errors { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion
    }
}
