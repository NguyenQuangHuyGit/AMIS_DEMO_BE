using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IAuthService
    {
        /// <summary>
        /// Hàm kiểm tra thông tin user có hợp lệ không
        /// </summary>
        /// <param name="email">Email đăng nhập user</param>
        /// <param name="password">Mật khẩu của user</param>
        /// <returns>Thông tin user => nếu thông tin user tồn tại || Null => nếu thông tin không tồn tại</returns>
        /// CreatedBy: QuangHuy (25/02/2024)
        public Task<UserModel?> CheckValidUserInformation(UserDto user);

        /// <summary>
        /// Hàm thực hiện xác thực người dùng và cấp quyền cho người dùng
        /// </summary>
        /// <param name="user">Dto người dùng</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (08/03/2024)
        public Task<UserModel> GetAuthentication(UserDto user);

        /// <summary>
        /// Hàm lấy lại quyền xác thực cho người dùng mà không cần đăng nhập lại
        /// </summary>
        /// <param name="oldAccessToken">Access token cũ</param>
        /// <param name="refreshToken">Refresh token của người dùng</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (08/03/2024)
        public Task RefreshAuthenticationForClient(string? oldAccessToken, string? refreshToken);

        /// <summary>
        /// Hàm xác thực và tiến hành thực hiện xử lý dữ liệu khi người dùng đăng xuất
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (08/03/2024)
        public Task ValidateTokenToLogout(string? token);

        /// <summary>
        /// Hàm lấy thông tin user hiện tại theo access token
        /// </summary>
        /// <param name="token">Access Token</param>
        /// <returns>Thông tin user tương ứng</returns>
        public Task<UserModel> GetCurrentUser(string? token); 
    }
}
