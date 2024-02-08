using Application.ServicesClients.DiscountsProduct.Models.Output;

namespace Application.ServicesClients.DiscountsProduct
{
    public interface IDiscountsProductServiceClient
    {
        Task<DiscountProductDto> GetDiscountByProduct(int productId);
    }
}
