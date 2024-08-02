using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Nhóm khách hàng
    /// </summary>
    public class CustomerGroup : BaseEntity
    {
        #region Properties
        // Id nhóm khách hàng
        [Key]
        public Guid CustomerGroupId { get; set; }

        // Tên nhóm khách hàng
        public string CustomerGroupName { get; set; } 
        #endregion
    }
}
