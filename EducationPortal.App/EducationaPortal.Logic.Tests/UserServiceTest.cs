using System.Linq.Expressions;
using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EducationaPortal.Service.Tests;

public class UserServiceTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IRepository<User, int>> mockRepository;
    private readonly Mock<PortalDbContext> mockContext;
    private readonly UserService userService;

    public UserServiceTest()
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockRepository = new Mock<IRepository<User, int>>();
        mockContext = new Mock<PortalDbContext>();
        mockUnitOfWork.Setup(u => u.Repository<User, int>()).Returns(mockRepository.Object);
        userService = new UserService(mockUnitOfWork.Object, mockContext.Object);
    }

    [Fact]
    public async Task InsertAsync_ValidUser_ReturnsTrue()
    {
        var userDto = new UserDTO
        {
            UserName = "testuser",
            FirstName = "Test",
            Surname = "User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };

        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await userService.InsertAsync(userDto);

        Assert.True(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<User>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_ExistingEmail_ReturnsFalse()
    {
        var userDto = new UserDTO
        {
            UserName = "testuser",
            FirstName = "Test",
            Surname = "User",
            Email = "existing@example.com",
            PhoneNumber = "1234567890"
        };

        var existingUser = new User
        {
            Id = 1,
            Email = "existing@example.com"
        };

        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(existingUser);

        var result = await userService.InsertAsync(userDto);

        Assert.False(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<User>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ExistingUser_ReturnsTrue()
    {
        var userDto = new UserDTO
        {
            Id = 1,
            UserName = "updateduser",
            FirstName = "Updated",
            Surname = "User",
            Email = "updated@example.com",
            PhoneNumber = "9876543210"
        };

        var existingUser = new User
        {
            Id = 1,
            UserName = "originaluser",
            FirstName = "Original",
            Surname = "User",
            Email = "original@example.com",
            PhoneNumber = "1234567890"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingUser);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await userService.UpdateAsync(userDto);

        Assert.True(result);
        Assert.Equal("updateduser", existingUser.UserName);
        Assert.Equal("Updated", existingUser.FirstName);
        Assert.Equal("User", existingUser.Surname);
        Assert.Equal("updated@example.com", existingUser.Email);
        Assert.Equal("9876543210", existingUser.PhoneNumber);
        mockRepository.Verify(r => r.UpdateAsync(existingUser), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentUser_ReturnsFalse()
    {
        var userDto = new UserDTO
        {
            Id = 999,
            UserName = "nonexistent",
            FirstName = "Non",
            Surname = "Existent",
            Email = "nonexistent@example.com",
            PhoneNumber = "0000000000"
        };

        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

        var result = await userService.UpdateAsync(userDto);

        Assert.False(result);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsTrue()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await userService.DeleteAsync(1);

        Assert.True(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SaveFails_ReturnsFalse()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await userService.DeleteAsync(1);

        Assert.False(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_ReturnsUserDto()
    {
        var user = new User
        {
            Id = 1,
            UserName = "testuser",
            FirstName = "Test",
            Surname = "User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        var result = await userService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("testuser", result.UserName);
        Assert.Equal("Test", result.FirstName);
        Assert.Equal("User", result.Surname);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("1234567890", result.PhoneNumber);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentUser_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

        var result = await userService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingUser_ReturnsUserDto()
    {
        var user = new User
        {
            Id = 1,
            UserName = "testuser",
            FirstName = "Test",
            Surname = "User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };

        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        var result = await userService.GetByEmailAsync("test@example.com");

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_NonExistentUser_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);

        var result = await userService.GetByEmailAsync("nonexistent@example.com");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new User { Id = 1, UserName = "user1", FirstName = "User", Surname = "One", Email = "user1@example.com", PhoneNumber = "1111111111" },
            new User { Id = 2, UserName = "user2", FirstName = "User", Surname = "Two", Email = "user2@example.com", PhoneNumber = "2222222222" }
        };

        mockRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(users);

        var result = await userService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.UserName == "user1");
        Assert.Contains(result, u => u.UserName == "user2");
    }

    [Fact]
    public async Task GetAllAsync_NoUsers_ReturnsEmptyCollection()
    {
        mockRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        var result = await userService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
