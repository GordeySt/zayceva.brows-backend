using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Domain;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.ApplicationUsers.Commands;

using static Testing;

public class SignupUserTests : BaseTestFixture
{
    private readonly SignupUserCommand _testCommand = new()
    {
        FirstName = "Admin",
        LastName = "Admin",
        Email = "administrator@localhost",
        Password = "Password123",
        PhoneNumber = "+375 (33) 322-22-22"
    };
    
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        // Arrange
        var command = new SignupUserCommand();

        // Act&Assert
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldNotBeSignedUpBefore()
    {
        // Arrange
        await CreateRoleAsync(AppRolesConstants.UserRole);
        await CreateUserAsync(
            new AppUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                Id = Guid.NewGuid(),
                UserName = "administrator@localhost", 
                Email = "administrator@localhost",
                SecurityStamp = Guid.NewGuid().ToString()
            }, 
        "Password123");

        // Act 
        var result = await SendAsync(_testCommand);
        
        // Assert
        result.Result.Should().Be(ApplicationResultType.InvalidData);
        result.Message.Should().Be(BadRequestExceptionMessageConstants.ExistedUser);
    }
    
    [Test]
    public async Task ShouldSignUpSuccessfully()
    {
        // Arrange
        await CreateRoleAsync(AppRolesConstants.UserRole);

        // Act 
        var result = await SendAsync(_testCommand);

        var createdUser = await SingleOrDefaultAsync<AppUser>(t => t.Email == _testCommand.Email);
        
        // Assert
        result.Result.Should().Be(ApplicationResultType.Success);
        createdUser?.Email.Should().Be(_testCommand.Email);
    }
}