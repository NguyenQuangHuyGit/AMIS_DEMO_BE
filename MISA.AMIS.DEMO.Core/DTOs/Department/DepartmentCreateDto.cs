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
    /// Dto phòng ban khi tạo mới
    /// </summary>
    public class DepartmentCreateDto
    {

        // Mã phòng ban
        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_DepartmentCodeRequired))]
        public string DepartmentCode { get; set; }

        // Tên phòng ban
        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_DepartmentNameRequired))]
        public string DepartmentName { get; set; }
    }
}
