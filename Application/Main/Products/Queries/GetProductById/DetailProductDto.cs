using Application._Common.Mapping;
using Domain.Main.Entities;

namespace Application.Main.Products.Queries.GetProductById
{
    public class DetailProductDto : IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double FinalPrice => Math.Round(Price * (100 - Discount) / 100, 2);
    }
}
