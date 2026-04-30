using FluentValidation;
using MediatR;
using StockApp.Shared.Errors;
using StockApp.Shared.Result;

namespace StockApp.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = failures
                .Select(f => Error.Validation(f.ErrorMessage))
                .ToList();

            return CreateFailureResult(errors);
        }

        return await next(cancellationToken);
    }

    private static TResponse CreateFailureResult(List<Error> errors)
    {
        var resultType = typeof(TResponse);

        if (resultType.IsGenericType &&
            resultType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericType = resultType.GetGenericArguments()[0];

            var method = typeof(Result<>)
                .MakeGenericType(genericType)
                .GetMethod(
                    nameof(Result<object>.Fail),
                    new[] { typeof(List<Error>) }
                );

            if (method == null)
                throw new InvalidOperationException("Fail(List<Error>) method not found");

            var result = method.Invoke(null, new object[] { errors });

            return (TResponse)result!;
        }

        throw new InvalidOperationException("TResponse must be Result<T>");
    }
}
