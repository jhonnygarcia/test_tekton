using FluentValidation;

namespace Application.Main.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(a => a.Name).NotEmpty();
            RuleFor(a => a.Description).NotEmpty();
            RuleFor(a => a.Stock).GreaterThanOrEqualTo(0);
            RuleFor(a => a.Price).GreaterThan(0);
            RuleFor(a => a.Status).InclusiveBetween((byte)0, (byte)1);
        }
    }
}
