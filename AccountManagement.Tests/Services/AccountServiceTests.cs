using AccountManagement.BLL.Services;
using AccountManagement.Domain.DTOs.AppUserDTOs;
using AccountManagement.Domain.Entities.Identity;
using AccountManagement.Domain.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace AccountManagement.Tests.Services;

public class AccountServiceTests
{
    private readonly AccountService _accountService;
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IJwtHandler> _mockJwtHandler;
    private readonly Mock<ILogger<AccountService>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;

    public AccountServiceTests()
    {
        // UserManager requires a store, which we mock for these tests
        var store = new Mock<IUserStore<AppUser>>();
        _mockUserManager = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
        _mockAuthService = new Mock<IAuthService>();
        _mockJwtHandler = new Mock<IJwtHandler>();
        _mockLogger = new Mock<ILogger<AccountService>>();
        _mockMapper = new Mock<IMapper>();

        _accountService = new AccountService(
            _mockAuthService.Object,
            _mockUserManager.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockJwtHandler.Object);
    }


    [Fact]
    public async Task CreateUser_ReturnsSuccess_WhenUserCreated()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "sanjabian.ho@gmail.com",
            UserName = "ZenMaxe",
            FirstName = "Hossein",
            Password = "5erAbbb1",
            ConfirmPassword = "5erAbbb1",
        };
        var id = Guid.NewGuid();
        var user = new AppUser
        {
            Id = id,
            Email = "sanjabian.ho@gmail.com",
            UserName = "ZenMaxe",
            FirstName = "Hossein",
        };
        var userDto = new UserDto
        {
            Id = id,
            Email = "sanjabian.ho@gmail.com",
            UserName = "ZenMaxe",
            FirstName = "Hossein",
            CreatedDate = DateTimeOffset.UtcNow
            /* ... properties ... */
        };


        _mockMapper.Setup(m => m.Map<AppUser>(createUserDto)).Returns(user);


        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);


        _mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<AppUser>())).Returns(userDto);

        // Act
        var result = await _accountService.CreateUser(createUserDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userDto, result.Result);
    }
}