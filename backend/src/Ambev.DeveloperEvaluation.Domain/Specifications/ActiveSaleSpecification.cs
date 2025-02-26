using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Specifications
{
    public class ActiveSaleSpecification : ISpecification<Sale>
    {
        public bool IsSatisfiedBy(Sale? sale)
        {
            return sale != null && 
                   sale.TotalValue > 0 && 
                   sale.Items != null && 
                   sale.Items.Any();
        }
    }
} 