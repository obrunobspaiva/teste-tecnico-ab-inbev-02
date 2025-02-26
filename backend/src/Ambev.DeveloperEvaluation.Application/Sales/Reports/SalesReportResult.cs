using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.Reports
{
    public class SalesReportResult
    {
        public string ReportType { get; set; }
        public List<SalesReportData> Data { get; set; }
    }

    public class SalesReportData
    {
        public string Period { get; set; } // Ex: "2024-02-01", "Semana 5", "Fevereiro 2024"
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
    }
}
