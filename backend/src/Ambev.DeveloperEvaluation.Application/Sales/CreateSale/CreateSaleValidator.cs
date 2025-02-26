using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(x => x.Customer).NotEmpty().WithMessage("O nome do cliente é obrigatório.");
            RuleFor(x => x.TotalValue).GreaterThan(0).WithMessage("O valor total deve ser maior que zero.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("A venda deve ter pelo menos um item.");
        }
    }
}
