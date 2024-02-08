using MediatR;

namespace Application.Main.Products.Commands.CreateProduct
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
