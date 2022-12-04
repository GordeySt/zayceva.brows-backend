using Application.Appointments.Commands.BookAppointments;
using Application.Appointments.Commands.CreateAppointments;
using Application.Appointments.Commands.DeleteAppointments;
using Application.Appointments.Commands.EditAppointments;
using Application.Appointments.Commands.UnbookAppointments;
using Application.Appointments.DTOs;
using Application.Appointments.Queries.GetAppointments;
using Application.Appointments.Queries.GetCurrentUserBookedAppointments;
using Application.Appointments.Queries.Params;
using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AppointmentsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AppointmentDto>>> 
        GetAppointments([FromQuery] AppointmentParams appointmentParams)
    {
        var result = await Mediator.Send(new GetAppointmentsQuery(appointmentParams));

        return result.Result is not ApplicationResultType.Success
            ? StatusCode((int) result.Result, result.Message)
            : Ok(result.Data);
    }
    
    [HttpGet("get-current-user-booked-appointments")]
    public async Task<ActionResult<List<AppointmentDto>>> GetCurrentUserBookedAppointments()
    {
        var result = await Mediator.Send(new GetCurrentUserBookedAppointmentsQuery());

        return result.Result is not ApplicationResultType.Success
            ? StatusCode((int) result.Result, result.Message)
            : Ok(result.Data);
    }
    
    [HttpPost]
    public async Task<ActionResult<CreateAppointmentCommand>> 
        CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        var result = await Mediator.Send(command);

        return result.Result is not ApplicationResultType.Success ? 
            StatusCode((int) result.Result, result.Message) : 
            CreatedAtAction(nameof(CreateAppointment), result.Data);
    }

    [HttpDelete("id/{id:guid}")]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        var result = await Mediator.Send(new DeleteAppointmentCommand(id));

        return result.Result is not ApplicationResultType.Success
            ? StatusCode((int) result.Result, result.Message)
            : NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> EditAppointment(EditAppointmentCommand command)
    {
        var result = await Mediator.Send(command);

        return result.Result is not ApplicationResultType.Success
            ? StatusCode((int) result.Result, result.Message)
            : NoContent();
    }

    [HttpPost("book-appointment/{id:guid}")]
    public async Task<IActionResult> BookAppointment(Guid id)
    {
        var result = await Mediator.Send(new BookAppointmentCommand(id));

        return result.Result is not ApplicationResultType.Success
            ? StatusCode((int) result.Result, result.Message)
            : NoContent();
    }
    
    [HttpPost("unbook-appointment/{id:guid}")]
    public async Task<IActionResult> UnookAppointment(Guid id)
    {
        var result = await Mediator.Send(new UnbookAppointmentCommand(id));

        return result.Result is not ApplicationResultType.Success
            ? StatusCode((int) result.Result, result.Message)
            : NoContent();
    }
}