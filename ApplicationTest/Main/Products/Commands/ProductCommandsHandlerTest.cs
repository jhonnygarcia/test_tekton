using Application.Main.Products.Commands.CreateProduct;
using Application.Main.Products.Commands.UpdateProduct;
using Application.Services;
using ApplicationTest._TestUtils;
using AutoMapper;
using DataAccess.Main.Context;
using Domain.Main.Entities;
using Domain.Main.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ApplicationTest.Main.Products.Commands
{
    public class ProductCommandsHandlerTest
    {
        [Fact]
        public async Task Create_product_successfully()
        {
            // Prepare
            var context = DbContextTestUtils.GetDbContextMock<MainContext>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, CreatedProductDto>();
                cfg.CreateMap<CreateProductCommand, Product>();
            });
            var mapper = new Mapper(config);

            var mockProductService = new Mock<IProductStatusService>();
            mockProductService.Setup(x => x.GetProductStatusAsync())
                .ReturnsAsync(new Dictionary<byte, string>
                {
                    { (byte)ProductStatus.Active, Guid.NewGuid().ToString() },
                    { (byte)ProductStatus.Inactive, Guid.NewGuid().ToString() },
                });

            var handler = new CreateProductCommandHandler(context, mapper, mockProductService.Object);
            var command = new CreateProductCommand
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Stock = 10,
                Price = 5,
                Status = (byte)ProductStatus.Active
            };
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var persisted = await context.Products.FirstOrDefaultAsync(x => x.Id == result.Id);

            Assert.NotNull(persisted);
            Assert.Equal(command.Name, persisted.Name);
            Assert.Equal(command.Description, persisted.Description);
            Assert.Equal(command.Status, persisted.Status);
            Assert.Equal(command.Stock, persisted.Stock);
            Assert.Equal(command.Price, persisted.Price);
        }
        
        [Fact]
        public async Task Update_product_successfully()
        {
            // Prepare
            var context = DbContextTestUtils.GetDbContextMock<MainContext>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, CreatedProductDto>();
                cfg.CreateMap<CreateProductCommand, Product>();
            });
            var mapper = new Mapper(config);            
            var handler = new UpdateProductCommandHandler(context, mapper);

            var @new = new Product
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Stock = 1,
                Price = 1,
                Status = (byte)ProductStatus.Active
            };
            context.Products.Add(@new);
            await context.SaveChangesAsync();

            var command = new UpdateProductCommand
            {
                Id = @new.Id,
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Stock = 10,
                Price = 5,
                Status = (byte)ProductStatus.Active
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var persisted = await context.Products.FirstOrDefaultAsync(x => x.Id == @new.Id);

            Assert.NotNull(persisted);
            Assert.Equal(command.Name, persisted.Name);
            Assert.Equal(command.Description, persisted.Description);
            Assert.Equal(command.Status, persisted.Status);
            Assert.Equal(command.Stock, persisted.Stock);
            Assert.Equal(command.Price, persisted.Price);
        }
    }
}