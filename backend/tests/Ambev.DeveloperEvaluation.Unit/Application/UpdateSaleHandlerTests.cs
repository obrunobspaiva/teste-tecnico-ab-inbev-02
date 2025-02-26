using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

public class UpdateSaleHandlerTests
{
    [Fact]
    public async Task Test_UpdateSale_Success()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new UpdateSaleHandler(mockRepository.Object, mockMapper.Object);
        
        var saleId = Guid.NewGuid();
        var existingSale = new Sale 
        { 
            Id = saleId,
            Customer = "Cliente Antigo",
            TotalValue = 100.00m
        };

        var command = new UpdateSaleCommand
        {
            Id = saleId,
            Customer = "Cliente Novo",
            TotalValue = 200.00m
        };

        mockRepository.Setup(r => r.GetByIdAsync(saleId))
            .ReturnsAsync(existingSale);
        
        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
            .Returns(Task.CompletedTask);

        mockMapper.Setup(m => m.Map(command, existingSale))
            .Returns(existingSale);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockRepository.Verify(r => r.GetByIdAsync(saleId), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(existingSale), Times.Once);
        mockMapper.Verify(m => m.Map(command, existingSale), Times.Once);
    }

    [Fact]
    public async Task Test_UpdateSale_NotFound()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new UpdateSaleHandler(mockRepository.Object, mockMapper.Object);
        var saleId = Guid.NewGuid();
        
        var command = new UpdateSaleCommand
        {
            Id = saleId,
            Customer = "Cliente Novo",
            TotalValue = 200.00m
        };

        mockRepository.Setup(r => r.GetByIdAsync(saleId))
            .ThrowsAsync(new InvalidOperationException("Venda não encontrada"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(command, CancellationToken.None));
        
        mockRepository.Verify(r => r.GetByIdAsync(saleId), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Sale>()), Times.Never);
        mockMapper.Verify(m => m.Map(command, It.IsAny<Sale>()), Times.Never);
    }
}
