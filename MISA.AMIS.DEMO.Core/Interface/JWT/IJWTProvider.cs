using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IJWTProvider
    {
        /// <summary>
        /// Hàm lấy token tương ứng
        /// </summary>
        /// <returns>Token JWT</returns>
        /// CreatedBy: QuangHuy (23/02/2024)
        public TokenModel GenerateToken(UserModel user);

        /// <summary>
        /// Lấy thông tin access token đã hết hạn
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Đối tượng trả về từ token</returns>
        /// CreatedBy: QuangHuy (25/02/2024)
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
