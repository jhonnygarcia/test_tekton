using Application.Main.Products.Dto;
using Application.Main.Products.Queries.GetProductById;
using Application.Main.Products.Queries.GetProducts;
using Application.Services;
using Application.ServicesClients.DiscountsProduct;
using Application.ServicesClients.DiscountsProduct.Models.Output;
using ApplicationTest._TestUtils;
using AutoMapper;
using DataAccess.Main.Context;
using Domain.Main.Entities;
using Domain.Main.Enums;
using Moq;

namespace ApplicationTest.Main.Products.Queries
{
    public class ProductQueriesHandlerTest
    {
        [Fact]
        public async Task Get_products_successfully()
        {
            // Prepare
            var context = DbContextTestUtils.GetDbContextMock<MainContext>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
            });
            var mapper = new Mapper(config);

            var mockProductService = new Mock<IProductStatusService>();
            mockProductService.Setup(x => x.GetProductStatusAsync())
                .ReturnsAsync(new Dictionary<byte, string>
                {
                    { (byte)ProductStatus.Active, Guid.NewGuid().ToString() },
                    { (byte)ProductStatus.Inactive, Guid.NewGuid().ToString() },
                });

            context.Products.Add(new Product { Name = "test 1" });
            context.Products.Add(new Product { Name = "test 2" });
            await context.SaveChangesAsync();

            var handler = new GetProductsQueryHandler(context, mapper, mockProductService.Object);
            var command = new GetProductsQuery("test");
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Get_product_by_id_successfully()
        {
            // Prepare
            var context = DbContextTestUtils.GetDbContextMock<MainContext>();    
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, DetailProductDto>();
            });
            var mapper = new Mapper(config);

            const double discount = 50;
            var mockDiscount = new Mock<IDiscountsProductServiceClient>();
            mockDiscount.Setup(x => x.GetDiscountByProduct(It.IsAny<int>()))
                .ReturnsAsync(new DiscountProductDto
                {
                    Discount = discount
                });

            context.Products.Add(new Product { Name = "test" });
            var betaProduct = new Product { Name = "beta", Price = 100 };
            context.Products.Add(betaProduct);
            context.Products.Add(new Product { Name = "gama" });
            await context.SaveChangesAsync();

            var mockProductService = new Mock<IProductStatusService>();
            mockProductService.Setup(x => x.GetProductStatusAsync())
                .ReturnsAsync(new Dictionary<byte, string>
                {
                    { (byte)ProductStatus.Active, Guid.NewGuid().ToString() },
                    { (byte)ProductStatus.Inactive, Guid.NewGuid().ToString() },
                });

            var handler = new GetProductByIdQueryHandler(context, mapper, 
                mockProductService.Object, mockDiscount.Object);
            var command = new GetProductByIdQuery(betaProduct.Id);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(betaProduct.Price * discount / 100, result.FinalPrice);
        }
    }
}