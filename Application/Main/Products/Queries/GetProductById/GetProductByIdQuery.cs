using MediatR;

namespace Application.Main.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<DetailProductDto>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
