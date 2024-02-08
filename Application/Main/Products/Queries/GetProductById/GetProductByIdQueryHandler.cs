using Application._Common.Exceptions;
using Application._Common.Mapping;
using Application.Services;
using Application.ServicesClients.DiscountsProduct;
using AutoMapper;
using Domain.Main.Entities;
using MediatR;

namespace Application.Main.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, DetailProductDto>
    {
        private readonly IMainContext _context;
        private readonly IMapper _mapper;
        private readonly IProductStatusService _productService;
        private readonly IDiscountsProductServiceClient _discountService;
        public GetProductByIdQueryHandler(IMainContext context, IMapper mapper,
            IProductStatusService productService, IDiscountsProductServiceClient discountService)
        {
            _context = context;
            _mapper = mapper;
            _productService = productService;
            _discountService = discountService;
        }

        public async Task<DetailProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            var discountProduct = await _discountService.GetDiscountByProduct(entity.Id);
            var productStatus = await _productService.GetProductStatusAsync();
            var result = _mapper.MapTo<DetailProductDto>(entity, new
            {
                StatusName = productStatus[entity.Status],
                discountProduct.Discount
            });

            return result;  
        }
    }
}
