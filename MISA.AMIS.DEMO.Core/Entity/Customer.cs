using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Khách hàng
    /// </summary>
    public class Customer : BaseEntity
    {
        #region Properties
        // Id khách hàng
        [Key]
        public Guid CustomerId { get; set; }

        // Mã khách hàng
        public string CustomerCode { get; set; }

        //Họ tên
        public string FullName { get; set; }

        // Email khách hàng
        public string? Email { get; set; }

        // Giới tính
        public Gender? Gender { get; set; }

        // Id nhóm khách hàng
        public Guid? CustomerGroupId { get; set; } 
        #endregion
    }
}
