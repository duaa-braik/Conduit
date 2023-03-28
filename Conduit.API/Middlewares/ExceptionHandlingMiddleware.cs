using Conduit.API.ErrorModels;
using Conduit.Domain.Exceptions;
using EntityFramework.Exceptions.Common;
using System.Net;

namespace Conduit.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private int statusCode;
        private string message;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (UniqueConstraintException)
            {
                statusCode = (int) HttpStatusCode.UnprocessableEntity;
                message = "Email Or Username are taken";
                await HandleException(httpContext, statusCode, message);
            }
            catch (LoginFailureException ex)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = ex.Message;
                await HandleException(httpContext, statusCode, message);
            }
            catch (FollowStatusMatchException ex)
            {
                statusCode = (int) HttpStatusCode.Conflict;
                message = ex.Message;
                await HandleException(httpContext, statusCode, message);
            }
        }

        private async Task HandleException(HttpContext httpContext, int statusCode, string errorMessage)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(
                new ErrorDetails() {
                    StatusCode = statusCode,
                    Message = errorMessage
                }.ToString());
        }
    }
}
