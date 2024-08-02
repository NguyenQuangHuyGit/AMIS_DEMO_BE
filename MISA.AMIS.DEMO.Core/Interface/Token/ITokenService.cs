using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface ITokenService
    {
        /// <summary>
        /// Hàm lấy access token theo thông tin user
        /// </summary>
        /// <param name="userInfo">Thông tin user modal</param>
        /// <returns>Chuỗi access token</returns>
        /// CreatedBy: QuangHuy (26/02/2024)
        public void GetAccessToken(UserModel userInfo);

        /// <summary>
        /// Hàm lấy RefreshToken
        /// </summary>
        /// <param name="userEntity">Thông tin user entity</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (26/02/2024)
        public Task GetRefreshToken(User userEntity);

        /// <summary>
        /// Hàm thu hồi RefreshToken và AccessToken
        /// </summary>
        /// <param name="token">Token chứa thông tin người dùng</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (26/02/2024)
        public Task RevokeTokens(User userEntity);
    }
}
