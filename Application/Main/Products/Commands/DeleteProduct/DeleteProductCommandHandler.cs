using AutoMapper;
using MediatR;
using Domain.Main.Entities;
using Microsoft.EntityFrameworkCore;
using Application._Common.Exceptions;

namespace Application.Main.Products.Commands.CreateProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IMainContext _context;
        private readonly IMapper _mapper;
        public DeleteProductCommandHandler(IMainContext mainContext, IMapper mapper)
        {
            _context = mainContext;
            _mapper = mapper;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
