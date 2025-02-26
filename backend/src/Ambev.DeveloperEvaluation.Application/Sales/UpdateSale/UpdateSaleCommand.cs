using MediatR;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Customer { get; set; }
        public decimal TotalValue { get; set; }
        public string StoreBranch { get; set; }
        public List<UpdateSaleItemDto> Items { get; set; }
    }

    public class UpdateSaleItemDto
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }
}
