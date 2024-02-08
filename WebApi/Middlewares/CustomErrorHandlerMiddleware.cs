using Application._Common.Exceptions;
using System.Net;
using System.Text.Json;
using WebApi.Models;

namespace WebApi.Middlewares
{
    public class CustomErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            HttpStatusCode code;
            ErrorResponse error;

            switch (exception)
            {
                case BusinessException businessException:
                    code = HttpStatusCode.Conflict;
                    error = new ErrorResponse
                    {
                        Code = businessException.Code,
                        Message = businessException.Message,
                        Details = businessException.Details
                    };
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    error = new ErrorResponse
                    {
                        Code = badRequestException.Code,
                        Message = badRequestException.Message,
                        Details = badRequestException.Details
                    };
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    error = new ErrorResponse
                    {
                        Code = notFoundException.Code,
                        Message = notFoundException.Message,
                        Details = notFoundException.Details
                    };
                    break;
                default:
                    throw exception;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}