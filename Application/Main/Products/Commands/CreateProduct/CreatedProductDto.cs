using Application._Common.Mapping;
using Domain.Main.Entities;

namespace Application.Main.Products.Commands.CreateProduct
{
    public class CreatedProductDto: IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StatusName { get; set; }
    }
}
