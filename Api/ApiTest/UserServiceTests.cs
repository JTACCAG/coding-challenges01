using Api.Application.DTOs;
using Api.Application.Enums;
using Api.Application.Exceptions;
using Api.Application.Repositories;
using Api.Application.Services;
using Api.Domain.Entities;
using MongoDB.Driver;
using Moq;

namespace ApiTest
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock
                .Setup(r => r.FindOne(It.IsAny<FilterDefinition<User>>(), It.IsAny<ProjectionDefinition<User>>()))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(r => r.FindAll(It.IsAny<FilterDefinition<User>>(), It.IsAny<ProjectionDefinition<User>>(), It.IsAny<SortDefinition<User>>()))
                .ReturnsAsync(new List<User>());

            _userRepositoryMock
                .Setup(r => r.Create(It.IsAny<CreateUserDto>()))
                .ReturnsAsync((CreateUserDto dto) => new User
                {
                    Id = "USR_TEST",
                    Email = dto.Email,
                    Fullname = dto.Fullname,
                    Role = RoleEnum.Regular
                });
            _service = new UserService(_userRepositoryMock.Object);
        }

        // ========================
        // Created
        // ========================
        [Fact]
        public async Task Created_ShouldReturnCreatedUser()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Email = "test@mail.com",
                Fullname = "Test User"
            };

            var user = new User
            {
                Id = "USR_1",
                Email = dto.Email,
                Fullname = dto.Fullname
            };

            _userRepositoryMock
                .Setup(r => r.Create(dto))
                .ReturnsAsync(user);

            // Act
            var result = await _service.Created(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Email, result.Email);
        }

        // ========================
        // ValidateByEmail
        // ========================
        [Fact]
        public async Task ValidateByEmail_WhenEmailExists_ThrowsException()
        {
            // Arrange
            var email = "exists@mail.com";

            _userRepositoryMock
                .Setup(r => r.FindOne(
                    It.IsAny<FilterDefinition<User>>(),
                    It.IsAny<ProjectionDefinition<User>>()))
                .ReturnsAsync(new User { Email = email });

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.ValidateByEmail(email));
        }

        [Fact]
        public async Task ValidateByEmail_WhenEmailDoesNotExist_DoesNotThrow()
        {
            // Arrange
            var email = "new@mail.com";

            _userRepositoryMock
                .Setup(r => r.FindOne(
                    It.IsAny<FilterDefinition<User>>(),
                    It.IsAny<ProjectionDefinition<User>>()))
                .ReturnsAsync((User?)null);

            // Act
            await _service.ValidateByEmail(email);

            // Assert
            Assert.True(true);
        }

        // ========================
        // FindOne
        // ========================
        [Fact]
        public async Task FindOne_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var userId = "USR_123";

            _userRepositoryMock
                .Setup(r => r.FindOne(
                    It.IsAny<FilterDefinition<User>>(),
                    It.IsAny<ProjectionDefinition<User>>()))
                .ReturnsAsync(new User { Id = userId });

            // Act
            var result = await _service.FindOne(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result!.Id);
        }

        [Fact]
        public async Task FindOne_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            _userRepositoryMock
                .Setup(r => r.FindOne(It.IsAny<FilterDefinition<User>>(), null))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _service.FindOne("NO_EXIST");

            // Assert
            Assert.Null(result);
        }

        // ========================
        // GetByEmail
        // ========================
        [Fact]
        public async Task GetByEmail_ReturnsProjectedUser()
        {
            // Arrange
            var email = "test@mail.com";

            var user = new User
            {
                Email = email,
                Fullname = "Test",
                Role = RoleEnum.Regular
            };

            _userRepositoryMock
                .Setup(r => r.FindOne(
                    It.IsAny<FilterDefinition<User>>(),
                    It.IsAny<ProjectionDefinition<User>>()))
                .ReturnsAsync(user);

            // Act
            var result = await _service.GetByEmail(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result!.Email);
            Assert.Equal(RoleEnum.Regular, result.Role);
        }
    }
}
