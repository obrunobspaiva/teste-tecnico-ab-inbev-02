using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class AuthenticateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly AuthenticateUserHandler _handler;

        public AuthenticateUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

            _handler = new AuthenticateUserHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenGeneratorMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Token_When_Credentials_Are_Valid()
        {
            var request = new AuthenticateUserCommand
            {
                Email = "teste@ambev.com",
                Password = "SenhaForte123"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email, CancellationToken.None))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(request.Password, user.Password))
                .Returns(true);

            _jwtTokenGeneratorMock.Setup(generator => generator.GenerateToken(user))
                .Returns("valid_token");

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("valid_token", result.Token);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Credentials_Are_Invalid()
        {
            var request = new AuthenticateUserCommand
            {
                Email = "teste@ambev.com",
                Password = "SenhaErrada"
            };

            var user = new User
            {
                Email = "teste@ambev.com",
                Username = "Teste",
                Password = "hashed_password"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email, CancellationToken.None))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(request.Password, user.Password))
                .Returns(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}
