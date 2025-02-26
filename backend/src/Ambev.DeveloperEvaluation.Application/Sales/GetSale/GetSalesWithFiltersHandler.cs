using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalesWithFiltersHandler : IRequestHandler<GetSalesWithFiltersQuery, List<GetSaleResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesWithFiltersHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<List<GetSaleResult>> Handle(GetSalesWithFiltersQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllAsync();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(request.Customer))
            {
                sales = sales.Where(s => s.Customer.Contains(request.Customer, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (request.StartDate.HasValue)
            {
                sales = sales.Where(s => s.SaleDate >= request.StartDate.Value).ToList();
            }

            if (request.EndDate.HasValue)
            {
                sales = sales.Where(s => s.SaleDate <= request.EndDate.Value).ToList();
            }

            // Aplicar ordenação
            sales = request.SortOrder.ToLower() == "asc"
                ? sales.OrderBy(s => s.SaleDate).ToList()
                : sales.OrderByDescending(s => s.SaleDate).ToList();

            // Aplicar paginação
            sales = sales
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return _mapper.Map<List<GetSaleResult>>(sales);
        }
    }
}
