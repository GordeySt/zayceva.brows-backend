/*using Application.ApplicationUsers.Commands.ConfirmEmails;
using Application.Common.Constants;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Domain;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.ApplicationUsers.Commands;

using static Testing;

public class ConfirmEmailTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        // Arrange
        var command = new ConfirmEmailCommand();

        // Act&Assert
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldNotConfirmEmailForNotFoundUser()
    {
        // Arrange
        var command = new ConfirmEmailCommand
        {
            Email = "test@test.com",
            Token = "TestToken"
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Result.Should().Be(ApplicationResultType.NotFound);
        result.Message.Should().Be(NotFoundExceptionMessageConstants.NotFoundItem);
    }

    [Test]
    public async Task ShouldConfirmEmailSuccessfully()
    {
        // Arrange
        var appUser = new AppUser
        {
            FirstName = "Admin",
            LastName = "Admin",
            Id = Guid.NewGuid(),
            UserName = "administrator@localhost.com",
            Email = "administrator@localhost.com",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        await CreateUserAsync(appUser, "admin123");

        var token = await GenerateEmailConfirmationTokenAsync(appUser);
        
        var command = new ConfirmEmailCommand
        {
            Email = "administrator@localhost.com",
            Token = token
        };

        // Act
        var result = await SendAsync(command);
        
        var confirmedUser = 
            await SingleOrDefaultAsync<AppUser>(t => t.Email == command.Email);

        // Assert
        result.Result.Should().Be(ApplicationResultType.Success);
        confirmedUser?.EmailConfirmed.Should().BeTrue();
    }
}*/