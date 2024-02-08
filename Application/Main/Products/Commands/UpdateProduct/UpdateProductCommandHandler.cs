using AutoMapper;
using MediatR;
using Domain.Main.Entities;
using Microsoft.EntityFrameworkCore;
using Application._Common.Exceptions;
using Application._Common.Mapping;

namespace Application.Main.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IMainContext _context;
        private readonly IMapper _mapper;
        public UpdateProductCommandHandler(IMainContext mainContext, IMapper mapper)
        {
            _context = mainContext;
            _mapper = mapper;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            _mapper.Assign(entity, request);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
