using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Entity vai trò
    /// </summary>
    public class Role : BaseEntity
    {
        // Id Vai trò
        public Guid RoleId { get; set; }

        // Tên vai trò
        public string RoleName { get; set; }
    }
}
