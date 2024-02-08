using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Models;

namespace WebApi.Controllers.Base
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());

        protected virtual IActionResult CustomErrorResult(HttpStatusCode statusCode)
        {
            return CustomErrorResult(statusCode, (ErrorResponse)null);
        }
        protected virtual IActionResult CustomErrorResult(HttpStatusCode statusCode, ErrorResponse error)
        {
            error = error ?? new ErrorResponse();
            var CustomErrorResult = new ErrorResponse
            {
                Code = error.Code ?? $"{(int)statusCode} {statusCode}",
                Message = error.Message ?? $"{statusCode}",
                Details = error.Details ?? new string[0],
            };
            return StatusCode((int)statusCode, CustomErrorResult);
        }
        protected virtual IActionResult CustomErrorResult(HttpStatusCode statusCode, string message)
        {
            return CustomErrorResult(statusCode, new ErrorResponse
            {
                Message = message
            });
        }
        protected virtual IActionResult CustomErrorResult(HttpStatusCode statusCode, string code, string message)
        {
            return CustomErrorResult(statusCode, new ErrorResponse
            {
                Code = code,
                Message = message
            });
        }
        protected virtual IActionResult CustomErrorResult(HttpStatusCode statusCode, IEnumerable<string> details)
        {
            return CustomErrorResult(statusCode, new ErrorResponse
            {
                Details = details?.ToArray() ?? new string[0]
            });
        }
        protected virtual IActionResult CustomErrorResult(HttpStatusCode statusCode, string code, 
            string message, IEnumerable<string> details)
        {
            return CustomErrorResult(statusCode, new ErrorResponse
            {
                Code = code,
                Message = message,
                Details = details?.ToArray() ?? new string[0]
            });
        }
    }
}
