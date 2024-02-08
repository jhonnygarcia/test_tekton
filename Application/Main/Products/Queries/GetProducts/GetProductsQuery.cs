using Application.Main.Products.Dto;
using MediatR;

namespace Application.Main.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string Name { get; set; }
        public GetProductsQuery(string name)
        {
            Name = name;
        }
    }
}
