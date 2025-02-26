using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalValue { get; set; }
        public string StoreBranch { get; set; }
        public List<SaleItem> Items { get; set; }
    }
}
