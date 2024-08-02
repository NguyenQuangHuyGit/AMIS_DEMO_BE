using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Phòng ban
    /// </summary>
    public class Department : BaseEntity
    {
        [Key]
        // Id phòng ban
        public Guid DepartmentId { get; set; }

        // Mã phòng ban
        public string DepartmentCode { get; set; }

        // Tên phòng ban
        public string DepartmentName { get; set;}
    }
}
