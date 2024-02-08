using Application._Common.Mapping;
using Domain.Main.Entities;
using MediatR;

namespace Application.Main.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<CreatedProductDto>, IMapTo<Product>
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
