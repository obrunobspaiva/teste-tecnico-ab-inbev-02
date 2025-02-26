using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = new Sale
            {
                SaleDate = DateTime.SpecifyKind(request.SaleDate, DateTimeKind.Utc),
                Customer = request.Customer,
                TotalValue = request.TotalValue,
                StoreBranch = request.StoreBranch,
                Items = _mapper.Map<List<SaleItem>>(request.Items)
            };

            await _saleRepository.AddAsync(sale);
            return _mapper.Map<CreateSaleResult>(sale);
        }
    }
}
