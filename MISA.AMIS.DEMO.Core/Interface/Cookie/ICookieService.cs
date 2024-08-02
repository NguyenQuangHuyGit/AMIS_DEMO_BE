using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface ICookieService
    {
        /// <summary>
        /// Hàm ghi token vao Cookie gửi qua Response
        /// </summary>
        /// <param name="token">Token Model</param>
        /// CreatedBy: QuangHuy (26/02/2024)
        public void WriteTokenToCookie(TokenModel token, string key);

        /// <summary>
        /// Hàm xóa 1 token trên cookie
        /// </summary>
        /// <param name="key">Key cần xóa</param>
        /// CreatedBy: QuangHuy (27/02/2024)
        public void DeleteTokenFromCookie(string key);
    }
}
