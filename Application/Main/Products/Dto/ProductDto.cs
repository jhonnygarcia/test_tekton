using Application._Common.Mapping;
using Domain.Main.Entities;

namespace Application.Main.Products.Dto
{
    public class ProductDto: IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
