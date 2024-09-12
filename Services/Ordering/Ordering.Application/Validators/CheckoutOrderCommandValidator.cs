
using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(u => u.UserName).NotEmpty().WithMessage("{UserName} is required").NotNull().MaximumLength(70).WithMessage("{UserName} must not exceed 70 characters");
            RuleFor(u => u.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required").GreaterThan(-1).WithMessage("{TotalPrice} should not be negative");
            RuleFor(u => u.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is required").NotNull().MaximumLength(70).WithMessage("{EmailAddress} must not exceed 70 characters");
            RuleFor(u => u.FirstName).NotEmpty().WithMessage("{FirstName} is required").NotNull().MaximumLength(70).WithMessage("{FirstName} must not exceed 70 characters");
            RuleFor(u => u.LastName).NotEmpty().WithMessage("{LastName} is required").NotNull().MaximumLength(70).WithMessage("{LastName} must not exceed 70 characters");


        }
    }
}
