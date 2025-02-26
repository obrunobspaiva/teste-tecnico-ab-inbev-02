using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.Reports
{
    public class GetSalesReportQuery : IRequest<SalesReportResult>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReportType { get; set; }
    }
}
