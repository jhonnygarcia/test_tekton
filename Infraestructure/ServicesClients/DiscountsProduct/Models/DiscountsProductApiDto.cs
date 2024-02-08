using Application._Common.Mapping;
using Application.ServicesClients.DiscountsProduct.Models.Output;
using System.Text.Json.Serialization;

namespace Infraestructure.ServicesClients.DiscountsProduct.Models
{
    public class DiscountsProductApiDto: IMapTo<DiscountProductDto>
    {
        [JsonPropertyName("id")]
        public int Id {  get; set; }

        [JsonPropertyName("discount")]
        public double Discount { get; set; }
    }
}
