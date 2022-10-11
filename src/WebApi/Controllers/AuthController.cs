using Application.ApplicationUsers.Commands.SignupUsers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUpUser(SignupUserCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}