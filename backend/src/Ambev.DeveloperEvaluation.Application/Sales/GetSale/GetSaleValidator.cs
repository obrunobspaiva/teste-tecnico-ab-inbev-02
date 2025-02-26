using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleValidator : AbstractValidator<GetSaleByIdQuery>
    {
        public GetSaleValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("O ID da venda é obrigatório.")
                .NotEqual(Guid.Empty).WithMessage("O ID da venda não pode ser um GUID vazio.");
        }
    }
}
