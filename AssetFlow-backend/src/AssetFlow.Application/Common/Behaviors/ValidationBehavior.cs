using FluentValidation;
using MediatR;

namespace AssetFlow.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // No validator registered for this request? Skip straight to the handler.
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        // Run every validator for this request type (usually one) in parallel.
        var results = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Flatten all failures from all validators into one list.
        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        // If anything failed, stop here — the handler never runs.
        if (failures.Count != 0)
            throw new ValidationException(failures);

        // All good — proceed to the handler.
        return await next();
    }
}