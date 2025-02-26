using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications
{
    public class ActiveUserSpecificationTests
    {
        private readonly ActiveUserSpecification _specification;

        public ActiveUserSpecificationTests()
        {
            _specification = new ActiveUserSpecification();
        }

        [Fact]
        public void Given_ActiveUser_When_Validated_Then_ShouldReturnTrue()
        {
            // Arrange
            var user = new User { Status = UserStatus.Active };

            // Act
            var result = _specification.IsSatisfiedBy(user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Given_SuspendedUser_When_Validated_Then_ShouldReturnFalse()
        {
            // Arrange
            var user = new User { Status = UserStatus.Suspended };

            // Act
            var result = _specification.IsSatisfiedBy(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Given_UnknownStatusUser_When_Validated_Then_ShouldReturnFalse()
        {
            // Arrange
            var user = new User { Status = UserStatus.Unknown };

            // Act
            var result = _specification.IsSatisfiedBy(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Given_NullUser_When_Validated_Then_ShouldReturnFalse()
        {
            // Act
            var result = _specification.IsSatisfiedBy(null);

            // Assert
            Assert.False(result);
        }
    }
}
