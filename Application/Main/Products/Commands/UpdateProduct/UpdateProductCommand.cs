using Application._Common.Mapping;
using Domain.Main.Entities;
using MediatR;

namespace Application.Main.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest, IMapTo<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public byte Status {  get; set; }
        public double Price { get; set; }
    }
}
