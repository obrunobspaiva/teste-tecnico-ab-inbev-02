using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;

public class DeleteSaleHandlerTests
{
    [Fact]
    public async Task Test_DeleteSale_Success()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var handler = new DeleteSaleHandler(mockRepository.Object);
        var command = new DeleteSaleCommand(Guid.NewGuid());

        mockRepository.Setup(r => r.DeleteAsync(command.SaleId))
            .Returns(Task.CompletedTask);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockRepository.Verify(r => r.DeleteAsync(command.SaleId), Times.Once);
    }

    [Fact]
    public async Task Test_DeleteSale_Failure_SaleNotFound()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var handler = new DeleteSaleHandler(mockRepository.Object);
        var command = new DeleteSaleCommand(Guid.NewGuid());

        mockRepository.Setup(r => r.DeleteAsync(command.SaleId))
            .ThrowsAsync(new InvalidOperationException("Venda não encontrada"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(command, CancellationToken.None));
        
        mockRepository.Verify(r => r.DeleteAsync(command.SaleId), Times.Once);
    }
}
