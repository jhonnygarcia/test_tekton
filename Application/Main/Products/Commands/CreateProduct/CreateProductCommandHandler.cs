using Application.Services;
using AutoMapper;
using Domain.Main.Entities;
using MediatR;

namespace Application.Main.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreatedProductDto>
    {
        private readonly IMainContext _context;
        private readonly IMapper _mapper;
        private readonly IProductStatusService _productService;
        public CreateProductCommandHandler(IMainContext mainContext, IMapper mapper,
            IProductStatusService productService)
        {
            _context = mainContext;
            _mapper = mapper;
            _productService = productService;
        }

        public async Task<CreatedProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request);
            
            _context.Products.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var productStatus = await _productService.GetProductStatusAsync();

            var result = _mapper.Map<CreatedProductDto>(entity);
            result.StatusName = productStatus[entity.Status];

            return result;
        }
    }
}
