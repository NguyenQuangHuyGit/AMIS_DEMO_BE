using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class TokenService : ITokenService
    {
        private readonly IJWTProvider _jwtProvider;
        private readonly ICookieService _cookieService;
        private readonly IUserRepository _userRepository;

        public TokenService(IJWTProvider jwtProvider, ICookieService cookieService, IUserRepository userRepository)
        {
            _jwtProvider = jwtProvider;
            _cookieService = cookieService;
            _userRepository = userRepository;
        }

        public void GetAccessToken(UserModel userInfo)
        {
            // Sinh access token ngẫu nhiên bằng JWT token
            var token = _jwtProvider.GenerateToken(userInfo);

            // Ghi token vào Cookie Response
            _cookieService.WriteTokenToCookie(token, "access_token");
        }

        public async Task GetRefreshToken(User userEntity)
        {
            // Sinh ngẫu nhiên refresh token
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // Cài đặt giá trị và hết hạn của Refresh Token
            userEntity.RefreshToken = refreshToken;
            userEntity.RefreshTokenExpiryTime = DateTime.Now.AddHours(24);

            // Cập nhật thông tin token vào DB
            await _userRepository.UpdateUserRefreshToken(userEntity);
            var refreshTokenModel = new TokenModel()
            {
                Value = refreshToken
            };

            // Ghi RefreshToken vào Cookie Response
            _cookieService.WriteTokenToCookie(refreshTokenModel, "refresh_token");
        }

        public async Task RevokeTokens(User userEntity)
        {
            // Cập nhật RefreshToken
            userEntity.RefreshToken = null;
            await _userRepository.UpdateUserRefreshToken(userEntity);

            // Xóa Token trên Cookie
            _cookieService.DeleteTokenFromCookie("access_token");
            _cookieService.DeleteTokenFromCookie("refresh_token");
        }
    }
}
