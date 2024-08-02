using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class TokenModel
    {
        // Chuỗi token
        public string Value { get; set; }

        // Ngày hết hạn
        public DateTime? ExpiredDate { get; set; }
    }
}
