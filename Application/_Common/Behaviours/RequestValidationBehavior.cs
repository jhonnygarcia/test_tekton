using Application._Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Application._Common.Behaviours
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>        
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .Select(f => $"{f.PropertyName}: {f.ErrorMessage}")
                .ToArray();

            if (errors.Any())
            {
                throw new BadRequestException("Something went wrong with your request. " +
                    "Please double-check and try again", errors);
            }

            return await next();
        }
    }
}
