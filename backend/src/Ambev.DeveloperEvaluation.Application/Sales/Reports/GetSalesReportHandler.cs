using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.Reports
{
    public class GetSalesReportHandler : IRequestHandler<GetSalesReportQuery, SalesReportResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesReportHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<SalesReportResult> Handle(GetSalesReportQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllAsync();
            sales = sales.Where(s => s.SaleDate >= request.StartDate && s.SaleDate <= request.EndDate).ToList();

            var reportData = new List<SalesReportData>();

            if (request.ReportType == "daily")
            {
                reportData = sales
                    .GroupBy(s => s.SaleDate.Date)
                    .Select(g => new SalesReportData
                    {
                        Period = g.Key.ToString("yyyy-MM-dd"),
                        TotalSales = g.Sum(s => s.TotalValue),
                        TotalOrders = g.Count()
                    }).ToList();
            }
            else if (request.ReportType == "weekly")
            {
                reportData = sales
                    .GroupBy(s => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(s.SaleDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                    .Select(g => new SalesReportData
                    {
                        Period = $"Semana {g.Key}",
                        TotalSales = g.Sum(s => s.TotalValue),
                        TotalOrders = g.Count()
                    }).ToList();
            }
            else if (request.ReportType == "monthly")
            {
                reportData = sales
                    .GroupBy(s => s.SaleDate.ToString("yyyy-MM"))
                    .Select(g => new SalesReportData
                    {
                        Period = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(g.Key.Split('-')[1])) + " " + g.Key.Split('-')[0],
                        TotalSales = g.Sum(s => s.TotalValue),
                        TotalOrders = g.Count()
                    }).ToList();
            }

            return new SalesReportResult
            {
                ReportType = request.ReportType,
                Data = reportData
            };
        }
    }
}
