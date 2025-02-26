using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleResult
    {
        public Guid Id { get; set; }
        public string Customer { get; set; }
        public decimal TotalValue { get; set; }
        public string StoreBranch { get; set; }
        public List<UpdateSaleItemDto> Items { get; set; }
    }
}
