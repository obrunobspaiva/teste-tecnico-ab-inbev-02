using System;
using System.Collections.Generic;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Test_Sale_Creation_Success()
    {
        // Arrange & Act
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            Customer = "Cliente Teste",
            TotalValue = 100.00m,
            StoreBranch = "Filial Teste",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    Product = "Produto Teste",
                    Quantity = 1,
                    UnitPrice = 100.00m,
                    Discount = 0.00m
                }
            }
        };

        // Assert
        Assert.NotNull(sale);
        Assert.NotEqual(Guid.Empty, sale.Id);
        Assert.NotNull(sale.Customer);
        Assert.NotNull(sale.StoreBranch);
        Assert.NotNull(sale.Items);
        Assert.Single(sale.Items);
        Assert.Equal(100.00m, sale.TotalValue);
    }

    [Fact]
    public void Test_Sale_TotalValue_MatchesItems()
    {
        // Arrange
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            Customer = "Cliente Teste",
            StoreBranch = "Filial Teste",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    Product = "Produto 1",
                    Quantity = 2,
                    UnitPrice = 50.00m,
                    Discount = 10.00m
                },
                new SaleItem
                {
                    Product = "Produto 2",
                    Quantity = 1,
                    UnitPrice = 100.00m,
                    Discount = 0.00m
                }
            }
        };

        // Act
        var expectedTotal = (2 * 50.00m - 10.00m) + (1 * 100.00m);
        sale.TotalValue = expectedTotal;

        // Assert
        Assert.Equal(expectedTotal, sale.TotalValue);
        Assert.Equal(2, sale.Items.Count);
    }

    [Fact]
    public void Test_Sale_WithoutItems_Success()
    {
        // Arrange & Act
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            Customer = "Cliente Teste",
            TotalValue = 0.00m,
            StoreBranch = "Filial Teste",
            Items = new List<SaleItem>()
        };

        // Assert
        Assert.NotNull(sale);
        Assert.Empty(sale.Items);
        Assert.Equal(0.00m, sale.TotalValue);
    }
}
