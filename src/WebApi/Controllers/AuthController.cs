using Application.ApplicationUsers.Commands.SignupUsers;
using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUpUser(SignupUserCommand command)
    {
        command.Origin = Request.Headers["origin"];
        
        var result = await Mediator.Send(command);

        if (result.Result is not ApplicationResultType.Success) 
            return StatusCode((int) result.Result, result.Message);
        
        return NoContent();
    }
}