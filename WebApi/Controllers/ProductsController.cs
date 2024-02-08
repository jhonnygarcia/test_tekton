using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application._Common.Mapping;
using Application.Main.Products.Commands.CreateProduct;
using Application.Main.Products.Commands.UpdateProduct;
using Application.Main.Products.Dto;
using Application.Main.Products.Queries.GetProductById;
using Application.Main.Products.Queries.GetProducts;
using WebApi.Models.RequestModel.Product;
using WebApi.Auth.Attributes;
using WebApi.Auth.Policies;

namespace WebApi.Controllers
{
    [ApiController]
    [Route(RoutePrefix)]
    [MainAuthorize(Policy = MainPolicies.PolicyEverythingAllowed)]
    public class ProductsController : ControllerBase
    {
        public const string RoutePrefix = "api/v1/products";
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get(string name = null)
        {
            var result = await _mediator.Send(new GetProductsQuery(name));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailProductDto>> Get(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CreatedProductDto>> Post(CreateProductCommand param)
        {
            var result = await _mediator.Send(param);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateProductReq req)
        {
            var param = _mapper.MapTo<UpdateProductCommand>(req, new { Id = id });
            await _mediator.Send(param);
            return Ok();
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return Ok();
        }
    }
}
