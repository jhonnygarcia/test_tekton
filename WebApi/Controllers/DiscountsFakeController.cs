using Application.ServicesClients.DiscountsProduct.Models.Output;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth.Attributes;

namespace WebApi.Controllers
{
    [ApiController]
    [Route(RoutePrefix)]
    [MainAuthorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DiscountsFakeController : ControllerBase
    {
        public const string RoutePrefix = "api/v1/discounts";        

        [HttpGet("{productId}")]
        public ActionResult<DiscountProductDto> Get(int productId)
        {
            double min = 5.5;
            double max = 20.5;
            return Ok(new DiscountProductDto
            {
                Id = productId,
                Discount = Math.Round(new Random().NextDouble() * (max - min) + min, 2)
            });
        }       
    }
}
