using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleByIdQuery : IRequest<GetSaleResult>
    {
        public Guid SaleId { get; set; }

        public GetSaleByIdQuery(Guid saleId)
        {
            SaleId = saleId;
        }
    }
}
