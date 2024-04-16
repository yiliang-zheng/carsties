using FluentValidation;
using MediatR;
using System;
using FluentResults;

namespace Application.Pipelines;

public class ValidationBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, Result<TResponse> > where TRequest : IRequest, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToList();

        if (errors.Any())
        {
            var exception = new ExceptionalError(new ValidationException(errors));
            return Result.Fail<TResponse>(exception);
        }

        var response = await next();

        return response;
    }
}