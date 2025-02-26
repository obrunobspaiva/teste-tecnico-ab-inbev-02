using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID da venda é obrigatório.")
                .NotEqual(Guid.Empty).WithMessage("O ID da venda não pode ser um GUID vazio.");

            RuleFor(x => x.Customer)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório.");

            RuleFor(x => x.TotalValue)
                .GreaterThan(0).WithMessage("O valor total da venda deve ser maior que zero.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("A venda deve ter pelo menos um item.");
        }
    }
}
