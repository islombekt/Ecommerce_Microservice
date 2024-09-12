﻿
using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviour
{
    // this will collect all fluent validators and run before handler
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators; 
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any()) 
            {
                var context = new ValidationContext<TRequest>(request);
                // this will run all the validation rules one by one and returns the validation result
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context,cancellationToken)));
                var failures = validationResults.SelectMany(c => c.Errors).Where(d => d != null).ToList();
                if (failures.Any()) 
                {
                    throw new ValidationException(failures);
                }
            }

            // on success
            return await next();
        }
    }
}
