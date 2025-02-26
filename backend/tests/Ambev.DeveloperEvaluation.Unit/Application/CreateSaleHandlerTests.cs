using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;

public class CreateSaleHandlerTests
{
    [Fact]
    public async Task Test_CreateSale_Success()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new CreateSaleHandler(mockRepository.Object, mockMapper.Object);

        var command = new CreateSaleCommand
        {
            SaleDate = DateTime.Now,
            Customer = "Cliente Teste",
            TotalValue = 100.00m,
            StoreBranch = "Filial Teste",
            Items = new List<CreateSaleItemDto>
            {
                new CreateSaleItemDto
                {
                    Product = "Produto Teste",
                    Quantity = 1,
                    UnitPrice = 100.00m,
                    Discount = 0.00m
                }
            }
        };

        mockMapper.Setup(m => m.Map<List<SaleItem>>(It.IsAny<List<CreateSaleItemDto>>()))
            .Returns(new List<SaleItem>());
        mockMapper.Setup(m => m.Map<CreateSaleResult>(It.IsAny<Sale>()))
            .Returns(new CreateSaleResult());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        mockRepository.Verify(r => r.AddAsync(It.IsAny<Sale>()), Times.Once);
    }

    [Fact]
    public async Task Test_CreateSale_Failure_InvalidData()
    {
        // Arrange
        var mockRepository = new Mock<ISaleRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new CreateSaleHandler(mockRepository.Object, mockMapper.Object);

        var command = new CreateSaleCommand
        {
            SaleDate = DateTime.Now,
            Customer = "", // Cliente inválido
            TotalValue = 0.00m, // Valor total inválido
            StoreBranch = "Filial Teste",
            Items = new List<CreateSaleItemDto>()  // Lista vazia de itens
        };

        mockMapper.Setup(m => m.Map<List<SaleItem>>(It.IsAny<List<CreateSaleItemDto>>()))
            .Throws(new InvalidOperationException("Dados inválidos"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(command, CancellationToken.None));
        
        mockRepository.Verify(r => r.AddAsync(It.IsAny<Sale>()), Times.Never);
    }
}
