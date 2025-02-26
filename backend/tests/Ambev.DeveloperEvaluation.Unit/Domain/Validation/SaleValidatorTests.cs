using System;
using System.Collections.Generic;
using Xunit;
using FluentValidation.TestHelper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleValidatorTests
    {
        private readonly SaleValidator _validator;

        public SaleValidatorTests()
        {
            _validator = new SaleValidator();
        }

        [Fact]
        public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
        {
            // Arrange
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

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Given_EmptyCustomer_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = "",
                TotalValue = 100.00m,
                StoreBranch = "Filial Teste",
                Items = new List<SaleItem> { new SaleItem() }
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Customer);
        }

        [Fact]
        public void Given_ZeroTotalValue_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = "Cliente Teste",
                TotalValue = 0,
                StoreBranch = "Filial Teste",
                Items = new List<SaleItem> { new SaleItem() }
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TotalValue);
        }

        [Fact]
        public void Given_NoItems_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = "Cliente Teste",
                TotalValue = 100.00m,
                StoreBranch = "Filial Teste",
                Items = new List<SaleItem>()
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Items);
        }

        [Fact]
        public void Given_InvalidItem_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
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
                        Product = "",
                        Quantity = 0,
                        UnitPrice = 0,
                        Discount = -1
                    }
                }
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor("Items[0].Product");
            result.ShouldHaveValidationErrorFor("Items[0].Quantity");
            result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
            result.ShouldHaveValidationErrorFor("Items[0].Discount");
        }

        [Fact]
        public void Given_EmptyStoreBranch_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = "Cliente Teste",
                TotalValue = 100.00m,
                StoreBranch = "",
                Items = new List<SaleItem> { new SaleItem() }
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StoreBranch);
        }

        [Fact]
        public void Given_NullItems_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = "Cliente Teste",
                TotalValue = 100.00m,
                StoreBranch = "Filial Teste",
                Items = null
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Items);
        }

        [Fact]
        public void Given_DiscountGreaterThanTotal_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
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
                        Discount = 150.00m // Desconto maior que o valor total
                    }
                }
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor("Items[0].Discount");
        }
    }
}
