using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

public class GetSaleHandlerTests
{
    [Fact]
    public async Task Test_GetSaleById_Success()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new GetSaleByIdHandler(mockRepository.Object, mockMapper.Object);
        var saleId = Guid.NewGuid();
        var query = new GetSaleByIdQuery(saleId);

        var sale = new Sale 
        { 
            Id = saleId,
            Customer = "Cliente Teste",
            TotalValue = 100.00m
        };
        
        mockRepository.Setup(r => r.GetByIdAsync(saleId))
            .ReturnsAsync(sale);
        
        mockMapper.Setup(m => m.Map<GetSaleResult>(sale))
            .Returns(new GetSaleResult 
            { 
                Id = sale.Id,
                Customer = sale.Customer,
                TotalValue = sale.TotalValue
            });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(saleId, result.Id);
        mockRepository.Verify(r => r.GetByIdAsync(saleId), Times.Once);
    }

    [Fact]
    public async Task Test_GetSaleById_NotFound()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new GetSaleByIdHandler(mockRepository.Object, mockMapper.Object);
        var saleId = Guid.NewGuid();
        var query = new GetSaleByIdQuery(saleId);

        mockRepository.Setup(r => r.GetByIdAsync(saleId))
            .ThrowsAsync(new InvalidOperationException("Venda não encontrada"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(query, CancellationToken.None));
        
        mockRepository.Verify(r => r.GetByIdAsync(saleId), Times.Once);
        mockMapper.Verify(m => m.Map<GetSaleResult>(It.IsAny<Sale>()), Times.Never);
    }
}
