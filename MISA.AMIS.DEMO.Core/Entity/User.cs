using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class User : BaseEntity
    {
        // Id người dùng
        public Guid UserId { get; set; }

        // Tên tài khoản người dùng
        public string UserName { get; set; }

        // Email người dùng
        public string Email { get; set; }

        // Số điện thoại
        public string PhoneNumber { get; set; }

        // Mật khẩu 
        public string Password { get; set; }

        // RefreshToken người dùng
        public string? RefreshToken { get; set; }

        // Ngày hết hạn RefreshToken
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Id Vai trò
        public Guid RoleId { get; set; }
    }
}
