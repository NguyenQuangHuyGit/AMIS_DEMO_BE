using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class EmployeeFillterDto
    {
        public IEnumerable<EmployeeDto> Employees { get; set; }

        public int TotalRecord { get; set; }
    }
}
