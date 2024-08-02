using MISA.AMIS.DEMO.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Dto khi tạo mới 1 Vị trí công việc (Position)
    /// </summary>
    public class PositionCreateDto
    {

        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_PositionCodeRequired))]
        // Mã công việc
        public string PositionCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_PositionNameRequired))]
        // Tên công việc
        public string PositionName { get; set; }
    }
}
