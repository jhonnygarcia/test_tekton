using MediatR;
using AutoMapper;
using Application.Main.Products.Dto;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Application.Services;

namespace Application.Main.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IMainContext _context;
        private readonly IMapper _mapper;
        private readonly IProductStatusService _productService;
        public GetProductsQueryHandler(IMainContext context, IMapper mapper, 
            IProductStatusService productService)
        {
            _context = context;
            _mapper = mapper;
            _productService = productService;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(x => x.Name.Contains(request.Name));
            }
            var productStatus = await _productService.GetProductStatusAsync();            
            var results = await query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync();
            results.ForEach(x =>
            {
                x.StatusName = productStatus[x.Status];
            });
            
            return results;
        }
    }
}
