using System;
using System.Collections.Generic;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications
{
    public class ActiveSaleSpecificationTests
    {
        private readonly ActiveSaleSpecification _specification;

        public ActiveSaleSpecificationTests()
        {
            _specification = new ActiveSaleSpecification();
        }

        [Fact]
        public void Given_ValidSale_When_Validated_Then_ShouldReturnTrue()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                TotalValue = 100.00m,
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = "Test Product",
                        Quantity = 1,
                        UnitPrice = 100.00m
                    }
                }
            };

            // Act
            var result = _specification.IsSatisfiedBy(sale);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Given_SaleWithZeroValue_When_Validated_Then_ShouldReturnFalse()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                TotalValue = 0,
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = "Test Product",
                        Quantity = 1,
                        UnitPrice = 0
                    }
                }
            };

            // Act
            var result = _specification.IsSatisfiedBy(sale);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Given_SaleWithNoItems_When_Validated_Then_ShouldReturnFalse()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                TotalValue = 100.00m,
                Items = new List<SaleItem>()
            };

            // Act
            var result = _specification.IsSatisfiedBy(sale);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Given_NullSale_When_Validated_Then_ShouldReturnFalse()
        {
            // Act
            var result = _specification.IsSatisfiedBy(null);

            // Assert
            Assert.False(result);
        }
    }
}
