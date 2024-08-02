using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.AMIS.DEMO.Core.Resources;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private readonly IJWTProvider _jwtProvider;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        #endregion

        #region Contructor
        public AuthService(IUserRepository userRepository, IJWTProvider jwtProvider, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        #endregion

        #region Methods
        public async Task<UserModel> GetAuthentication(UserDto user)
        {
            // Kiểm tra thông tin user
            var userInfo = await CheckValidUserInformation(user);

            // Nếu user hợp lệ thì tiếp tục
            if (userInfo != null)
            {
                var userEntity = _mapper.Map<User>(userInfo);
                _tokenService.GetAccessToken(userInfo);
                await _tokenService.GetRefreshToken(userEntity);
                return userInfo;
            }
            else
            {
                // Thông tin không hợp lệ thì trả về Exception
                throw new InvalidUserLoginException()
                {
                    UserMessage = MISAResources.InValidMsg_LoginInfomation,
                    DevMessage = MISAResources.InValidMsg_InValidInput
                };
            }
        }

        public async Task ValidateTokenToLogout(string? token)
        {
            if (token != null)
            {
                // Lấy tên đăng nhập từ Access Token
                var username = ValidateUserFromToken(token);

                // Lấy thông tin User từ DB
                var userInfo = await _userRepository.GetUserModel(username);
                var userEntity = _mapper.Map<User>(userInfo);

                // Kiểm tra User có tồn tại không
                if (userInfo != null)
                {
                    // Thu hồi token
                    await _tokenService.RevokeTokens(userEntity);
                }
                else
                {
                    // Nếu không tìm thấy thông tin user từ DB trả về Exception
                    throw new AuthenticationTimeout()
                    {
                        DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                        UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                    };
                }
            }
            else
            {
                // Nếu token truyền vào rỗng trả về Exception
                throw new AuthenticationTimeout()
                {
                    DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                    UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                };
            }
        }

        public async Task RefreshAuthenticationForClient(string? oldAccessToken, string? refreshToken)
        {
            // Kiểm tra Access Token và Refresh Token có hợp lệ không
            if (oldAccessToken != null && refreshToken != null)
            {
                // Lấy tên đăgn nhập của người dùng từ Access Token đã hết hạn
                var username = ValidateUserFromToken(oldAccessToken);

                // Lấy thông tin user từ tên đăng nhập
                var userInfo = await _userRepository.GetUserModel(username);
                var userEntity = _mapper.Map<User>(userInfo);

                // Nếu user không tồn tại || refresh token ko khớp || refresh token đã hết hạn thì trả về Exception
                if (userInfo == null || userInfo.RefreshToken != refreshToken || userInfo.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    throw new AuthenticationTimeout()
                    {
                        DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                        UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                    };
                }

                // Lấy Token mới
                _tokenService.GetAccessToken(userInfo);
                await _tokenService.GetRefreshToken(userEntity);
            }
            else
            {
                // Trả về Exception nếu Access Token hoặc Refresh Token truyền vào trống
                throw new AuthenticationTimeout
                {
                    DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                    UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                };
            }
        }

        public async Task<UserModel> GetCurrentUser(string? token)
        {
            // Kiểm tra xem Request có access token không
            if (token != null)
            {
                var username = ValidateUserFromToken(token);
                var userInfo = await _userRepository.GetUserModel(username);
                if (userInfo != null)
                {
                    return userInfo;
                }
                else
                {
                    // Nếu tìm thấy thông tin User từ DB => trả về Exception
                    throw new AuthenticationTimeout()
                    {
                        DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                        UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                    };
                }
            }
            else
            {
                // Nếu không có token trả về Exception
                throw new AuthenticationTimeout()
                {
                    DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                    UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                };
            }

        }

        public async Task<UserModel?> CheckValidUserInformation(UserDto user)
        {
            var userInfo = await _userRepository.GetUserModel(user.UserName);
            if (userInfo != null)
            {
                if (userInfo.Password == user.Password)
                {
                    return userInfo;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Hàm kiểm tra tính hợp lệ của access token
        /// </summary>
        /// <param name="token">Access Token</param>
        /// <returns>Tên đăng nhập của người dùng</returns>
        /// <exception cref="AuthenticationTimeout">Trả về ngoại lệ nếu xảy ra lỗi</exception>
        /// CreatedBy: QuangHuy (27/02/2024)
        private string ValidateUserFromToken(string token)
        {
            // Lấy định danh người dùng
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(token);
            if (principal != null)
            {
                // Lấy tên đăng nhập
                var username = principal?.Identity?.Name;
                if (username != null)
                {
                    return username;
                }
                else
                {
                    // Nếu không lấy tên đăng nhập => token không hợp lệ => trả về Exception
                    throw new AuthenticationTimeout()
                    {
                        DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                        UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                    };
                }
            }
            else
            {
                // Nếu không lấy được định danh => token không hợp lệ => trả về Exception
                throw new AuthenticationTimeout()
                {
                    DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                    UserMessage = MISAResources.InValidMsg_UnauthorizedUser
                };
            }
        }
        #endregion
    }
}
