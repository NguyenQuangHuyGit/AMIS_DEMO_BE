using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class EmployeeFillterModel
    {
        // Danh sách nhân viên
        public IEnumerable<EmployeeModel> Employees { get; set; }

        // Tổng số bản ghi của lần lọc đó
        public int TotalRecord {  get; set; }
    }
}
