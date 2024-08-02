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
    /// User DTO
    /// </summary>
    public class UserDto
    {
        // Tài khoản đăng nhập
        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_UserNameRequired))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_PasswordRequired))]
        // Mật khẩu
        public string Password { get; set; }    
    }
}
