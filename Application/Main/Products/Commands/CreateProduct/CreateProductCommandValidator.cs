using FluentValidation;

namespace Application.Main.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(a => a.Name).NotEmpty();
            RuleFor(a => a.Description).NotEmpty();
            RuleFor(a => a.Stock).GreaterThanOrEqualTo(0);
            RuleFor(a => a.Price).GreaterThan(0);
            RuleFor(a => a.Status).InclusiveBetween((byte)0, (byte)1);
        }
    }
}
