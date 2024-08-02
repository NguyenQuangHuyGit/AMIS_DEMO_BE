using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class CookieService : ICookieService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor; 
        #endregion

        #region Contructor
        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        } 
        #endregion

        #region Methods
        public void DeleteTokenFromCookie(string key)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddSeconds(-1)
            };
            _httpContextAccessor?.HttpContext?.Response.Cookies.Delete(key, cookieOptions);
        }

        public void WriteTokenToCookie(TokenModel token, string key)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(24),
            };

            _httpContextAccessor?.HttpContext?.Response.Cookies.Append(key, token.Value, cookieOptions);
        } 
        #endregion
    }
}
