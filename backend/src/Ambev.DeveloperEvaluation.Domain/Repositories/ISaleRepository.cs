using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task AddAsync(Sale sale);
        Task<Sale> GetByIdAsync(Guid id);
        Task<List<Sale>> GetAllAsync();
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Guid id);
        Task<List<Sale>> GetFilteredSalesAsync(string? customer, DateTime? startDate, DateTime? endDate, int page, int pageSize, string sortOrder);
    }


}
