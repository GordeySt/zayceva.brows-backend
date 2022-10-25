using Application.ApplicationUsers.Commands.ConfirmEmails;
using Application.ApplicationUsers.Commands.SignupUsers;
using Application.ApplicationUsers.Queries.ResendEmailConfirmations;
using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : ApiControllerBase
{
    /// <summary>
    /// User account signing up
    /// </summary>
    /// <param name="command">Sign up data.</param>
    /// <response code="204">User signed up successfully.</response>
    /// <response code="400">Unable to create user due to existence.</response>
    /// <returns>
    /// NoContent object result
    /// </returns>
    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUpUser([FromBody] SignupUserCommand command)
    {
        command.Origin = Request.Headers["origin"];
        
        var result = await Mediator.Send(command);

        if (result.Result is not ApplicationResultType.Success) 
            return StatusCode((int) result.Result, result.Message);
        
        return NoContent();
    }

    /// <summary>
    /// User account email confirmation
    /// </summary>
    /// <param name="command">Email confirmation data.</param>
    /// <response code="204">Email confirmed successfully.</response>
    /// <response code="400">Problem confirming email.</response>
    /// <response code="404">User to confirm email for not found.</response>
    /// <returns>
    /// NoContent object result
    /// </returns>
    [HttpPost("confirm-email")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.Result is not ApplicationResultType.Success)
            return StatusCode((int) result.Result, result.Message);

        return NoContent();
    }

    /// <summary>
    /// User account email confirmation
    /// </summary>
    /// <param name="query">Email resend data.</param>
    /// <response code="204">Email resent successfully.</response>
    /// <response code="404">User to resend email confirmation for not found.</response>
    /// <returns>
    /// NoContent object result
    /// </returns>
    [HttpGet("resend-confirmation-email")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResendConfirmationEmail([FromQuery] ResendEmailConfirmationQuery query)
    {
        query.Origin = Request.Headers["origin"];

        var result = await Mediator.Send(query);
        
        if (result.Result is not ApplicationResultType.Success)
            return StatusCode((int) result.Result, result.Message);

        return NoContent();
    }
}