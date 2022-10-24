using Application.ApplicationUsers.Commands.ConfirmEmails;
using Application.ApplicationUsers.Commands.SignupUsers;
using Application.ApplicationUsers.Queries.ResendEmailConfirmations;
using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpUser([FromBody] SignupUserCommand command)
    {
        command.Origin = Request.Headers["origin"];
        
        var result = await Mediator.Send(command);

        if (result.Result is not ApplicationResultType.Success) 
            return StatusCode((int) result.Result, result.Message);
        
        return NoContent();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.Result is not ApplicationResultType.Success)
            return StatusCode((int) result.Result, result.Message);

        return NoContent();
    }

    [HttpGet("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromQuery] ResendEmailConfirmationQuery query)
    {
        query.Origin = Request.Headers["origin"];

        var result = await Mediator.Send(query);
        
        if (result.Result is not ApplicationResultType.Success)
            return StatusCode((int) result.Result, result.Message);

        return NoContent();
    }
}