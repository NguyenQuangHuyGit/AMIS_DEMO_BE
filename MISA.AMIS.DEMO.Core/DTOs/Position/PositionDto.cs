using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Dto Công việc, vị trí (Position) khi lấy ra
    /// </summary>
    public class PositionDto
    {
        [Key]
        // Mã vị trí / công việc
        public Guid PositionId { get; set; }

        // Mã công việc
        public string PositionCode { get; set; }

        // Tên công việc
        public string PositionName { get; set; }
    }
}
