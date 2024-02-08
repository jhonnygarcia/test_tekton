using AutoMapper;
using System.Net.Http.Json;
using Application.ServicesClients.DiscountsProduct;
using Application.ServicesClients.DiscountsProduct.Models.Output;
using Application.Utility;
using Infraestructure.ServicesClients.DiscountsProduct.Models;

namespace Infraestructure.ServicesClients.DiscountsProduct
{
    public class DiscountsProductServiceClient : IDiscountsProductServiceClient
    {
        private const string GetDiscountPath = "api/v1/discounts";
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public DiscountsProductServiceClient(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient(InfraestructureConfiguration.DiscountConfig);
            _mapper = mapper;
        }
        public async Task<DiscountProductDto> GetDiscountByProduct(int productId)
        {
            var requestUri = $"{GetDiscountPath}/{productId}".RelativeUri();
            var  response = await _httpClient.GetFromJsonAsync<DiscountsProductApiDto>(requestUri);
            return _mapper.Map<DiscountProductDto>(response);
        }
    }
}
