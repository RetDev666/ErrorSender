using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace ErrSendWebApi.ExceptionMidlevare
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            var exStat = new ExecutionStatus();
            exStat.Status = "ER";

            switch (exception)
            {
                /*Розібратись написати перехоплення кастомних виключень
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    foreach (var item in validationException.Errors)
                    {
                        exStat.Errors.Add(item.ErrorMessage);
                    }

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case AddDelRegException:
                    code = HttpStatusCode.Conflict;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case SecurityTokenException:
                    code = HttpStatusCode.Unauthorized;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case LoginExistException:
                    code = HttpStatusCode.Forbidden;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case InvalidUsernameOrPasswordException:
                    code = HttpStatusCode.Unauthorized;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case RefreshTokenInvalidOrExpiredException:
                    code = HttpStatusCode.Unauthorized;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case ExecSpException:
                    code = HttpStatusCode.Conflict;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;
                case InvalidOperationException:
                    code = HttpStatusCode.Conflict;
                    exStat.Errors.Add(exception.Message);

                    result = JsonSerializer.Serialize(exStat);
                    break;*/
                   

            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { errpr = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
