using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(sale => sale.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required.");

            RuleFor(sale => sale.SaleDate)
                .NotEmpty()
                .WithMessage("Sale date is required.");

            RuleFor(sale => sale.Customer)
                .NotEmpty()
                .WithMessage("Customer name is required.")
                .MaximumLength(100)
                .WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(sale => sale.TotalValue)
                .GreaterThan(0)
                .WithMessage("Total value must be greater than zero.");

            RuleFor(sale => sale.StoreBranch)
                .NotEmpty()
                .WithMessage("Store branch is required.")
                .MaximumLength(100)
                .WithMessage("Store branch name cannot exceed 100 characters.");

            RuleFor(sale => sale.Items)
                .NotNull()
                .WithMessage("Items list cannot be null.")
                .Must(items => items != null && items.Any())
                .WithMessage("Sale must have at least one item.");

            RuleForEach(sale => sale.Items)
                .SetValidator(new SaleItemValidator());
        }
    }

    public class SaleItemValidator : AbstractValidator<SaleItem>
    {
        public SaleItemValidator()
        {
            RuleFor(item => item.Product)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");

            RuleFor(item => item.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Unit price must be greater than zero.");

            RuleFor(item => item.Discount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Discount cannot be negative.")
                .LessThanOrEqualTo(item => item.UnitPrice * item.Quantity)
                .WithMessage("Discount cannot be greater than the total item value.");
        }
    }
} 