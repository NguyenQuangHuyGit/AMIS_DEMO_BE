using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IUserRepository : IBaseRepository<UserModel>
    {
        /// <summary>
        /// Hàm Lấy thông tin User theo email || phoneNumber
        /// </summary>
        /// <param name="accont">Tìa khoản đăng nhập: Email || PhoneNumber</param>
        /// <returns>Thông tin tài khoản || Null</returns>
        /// CreatedBy: QuangHuy (25/02/2024)
        public Task<UserModel?> GetUserModel(string account);

        /// <summary>
        /// Hàm cập nhật RefreshToken của người dùng khi hết hạn
        /// </summary>
        /// <param name="user">Thông tin User</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (26/02/2024)
        public Task UpdateUserRefreshToken(User user);
    }
}
