using MediatR;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalValue { get; set; }
        public string StoreBranch { get; set; }
        public List<CreateSaleItemDto> Items { get; set; }
    }

    public class CreateSaleItemDto
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }
}
