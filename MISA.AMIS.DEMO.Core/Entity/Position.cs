using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Vị trí công việc
    /// </summary>
    public class Position : BaseEntity
    {
        [Key]
        // Guid Id của công việc, vị trí
        public Guid PositionId { get; set; }

        // Mã của công việc, vị trí
        public string PositionCode { get; set; }

        // Tên của công việc, vị trí
        public string PositionName { get; set; }
    }
}
