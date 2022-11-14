﻿using Application.ApplicationUsers.Queries.GetCurrentUsers;
using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet("get-current-user")]
    public async Task<IActionResult> ResendConfirmationEmail()
    {
        var result = await Mediator.Send(new GetCurrentUserQuery());
        
        return result.Result is not ApplicationResultType.Success ? 
            StatusCode((int) result.Result, result.Message) : Ok(result.Data);
    }
}