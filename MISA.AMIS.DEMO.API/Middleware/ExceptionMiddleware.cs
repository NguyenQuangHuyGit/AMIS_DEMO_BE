using Microsoft.IdentityModel.Tokens;
using MISA.AMIS.DEMO.Core;
using MISA.AMIS.DEMO.Core.Resources;
using System;
using System.Net;

namespace MISA.AMIS.DEMO.API
{
    public class ExceptionMiddleware
    {
        #region Fields
        private readonly RequestDelegate _requestDelegate;
        #endregion

        #region Contructor
        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Hàm xử lý lỗi trong ứng dụng trên Middleware
        /// </summary>
        /// <param name="context">Http response</param>
        /// <param name="ex">Exception bắt được</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (08/01/2024)
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            switch (ex)
            {
                case DuplicateException:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    await context.Response.WriteAsync(
                        text: new MISAException()
                        {
                            UserMessage = ((DuplicateException)ex).UserMessage,
                            DevMessage = ex.Message,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = ex.HelpLink,
                            Errors = ((DuplicateException)ex).Errors
                        }.ToString() ?? ""
                    );
                    break;
                case NotExistException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(
                        text: new MISAException()
                        {
                            UserMessage = ((NotExistException)ex).UserMessage,
                            DevMessage = ex.Message,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = ex.HelpLink,
                            Errors = ((NotExistException)ex).Errors
                        }.ToString() ?? ""
                    );
                    break;
                case FileException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(
                        text: new MISAException()
                        {
                            UserMessage = ex.Message,
                            DevMessage = ex.Message,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = ex.HelpLink,
                        }.ToString() ?? ""
                    );
                    break;
                case InvalidUserLoginException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(
                        text: new MISAException()
                        {
                            UserMessage = ((InvalidUserLoginException)ex).UserMessage,
                            DevMessage = ((InvalidUserLoginException)ex).DevMessage,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = ex.HelpLink,
                        }.ToString() ?? ""
                    );
                    break;
                case AuthenticationTimeout:
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddSeconds(-1)
                    };
                    context.Response.Cookies.Delete("access_token", cookieOptions);
                    context.Response.Cookies.Delete("refresh_token", cookieOptions);
                    context.Response.StatusCode = StatusCodes.Status419AuthenticationTimeout;
                    await context.Response.WriteAsync(
                        text: new MISAException()
                        {
                            UserMessage = ((AuthenticationTimeout)ex).UserMessage,
                            DevMessage = ((AuthenticationTimeout)ex).DevMessage,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = ex.HelpLink,
                        }.ToString() ?? ""
                    );
                    break;
                case SessionTimeOutException:
                    context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                    await context.Response.WriteAsync(
                        text: new MISAException()
                        {
                            UserMessage = ((SessionTimeOutException)ex).UserMessage,
                            DevMessage = ((SessionTimeOutException)ex).DevMessage,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = ex.HelpLink,
                        }.ToString() ?? ""
                    );
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(
                        text: new MISAException()
                        {
                            UserMessage = MISAResources.InValidMsg_SystemError,
                            DevMessage = ex.Message,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = ex.HelpLink,
                        }.ToString() ?? ""
                    );
                    break;
            }
        }
        #endregion
    }
}
