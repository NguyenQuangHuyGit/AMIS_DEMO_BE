using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using MISA.AMIS.DEMO.Core;
using Microsoft.AspNetCore.Authorization;

namespace MISA.AMIS.DEMO.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountsController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Hàm nhận request lấy thông tin user hiện tại đang sử dụng
        /// </summary>
        /// <returns>Trạng thái 200 && Thông tin user tương ứng</returns>
        /// CreatedBy: QuangHuy (27/02/2024)
        [HttpGet]
        public async Task<ActionResult> GetUser()
        {
            var accessToken = Request.Cookies["access_token"];
            var result = await _authService.GetCurrentUser(accessToken);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Hàm nhận request đăng nhập và trả về access token cho người dùng
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Trang thái 200, Tên đăng nhập User</returns>
        /// CreatedBy: QuangHuy (25/02/2024)
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserDto user)
        {
            var result = await _authService.GetAuthentication(user);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Hàm nhận yêu cầu làm mới Access Token khi đã hết hạn
        /// </summary>
        /// <param name="user">Thông tin user</param>
        /// <returns>Trang thái 204</returns>
        /// CreatedBy: QuangHuy (26/02/2024)
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshAccessToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            var oldAccessToken = Request.Cookies["access_token"];
            await _authService.RefreshAuthenticationForClient(oldAccessToken, refreshToken);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Hàm nhận yêu cầu đăng xuất của người dùng
        /// Thu hồi RefreshToken
        /// </summary>
        /// <param name="username">Tên người dùng</param>
        /// <returns>Trạng thái 204</returns>
        /// CreatedBy: QuangHuy (26/02/2024)
        [HttpPost("Revoke")]
        public async Task<ActionResult> Revoke()
        {
            var token = Request.Cookies["access_token"];
            await _authService.ValidateTokenToLogout(token);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
