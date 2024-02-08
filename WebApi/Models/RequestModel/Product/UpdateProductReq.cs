using Application._Common.Mapping;
using Application.Main.Products.Commands.UpdateProduct;

namespace WebApi.Models.RequestModel.Product
{
    public class UpdateProductReq: IMapTo<UpdateProductCommand>
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public double Price { get; set; }
    }
}
