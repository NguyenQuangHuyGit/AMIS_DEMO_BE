using System.Globalization;

namespace MISA.AMIS.DEMO.API
{
    public class AuthenticationMiddleware
    {
        #region Fields
        private readonly RequestDelegate _requestDelegate;
        #endregion

        #region Contructor
        public AuthenticationMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        #endregion

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Cookies["access_token"];
            if (token != null)
            {
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }
            await _requestDelegate(context);
        }
    }
}
