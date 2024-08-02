using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class JWTProvider : IJWTProvider
    {
        #region Fields
        private readonly IConfiguration _configuration; 
        #endregion

        #region Contructor
        public JWTProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        } 
        #endregion

        #region Methods
        public TokenModel GenerateToken(UserModel user)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = GetToken(authClaims);
            var tokenModel = new TokenModel()
            {
                Value = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiredDate = token.ValidTo,
            };
            return tokenModel;
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Invalid Token");
            }
            return principal;
        }

        /// <summary>
        /// Hàm sinh token với các thông tin của đối tượng
        /// </summary>
        /// <param name="authClaims">Danh sách thông tin muốn thêm vào token</param>
        /// <returns>Token tương ứng</returns>
        /// CreatedBy: QuangHuy (23/02/2024)
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        } 
        #endregion
    }
}
