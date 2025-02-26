using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleCommand : IRequest
    {
        public Guid SaleId { get; set; }

        public DeleteSaleCommand(Guid saleId)
        {
            SaleId = saleId;
        }
    }
}
