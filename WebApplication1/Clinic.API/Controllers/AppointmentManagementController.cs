using Microsoft.AspNetCore.Mvc;
using Clinic.Services.Models.Appointment;
using Clinic.Services.Services.Appointments;

namespace Clinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentManagementController : ControllerBase
{
    private readonly IAppointmentService _service;

    public AppointmentManagementController(IAppointmentService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentResponse>> Create([FromBody] CreateAppointmentRequest request, CancellationToken ct)
    {
        var result = await _service.CreateAsync(request, ct);
        if (result == null)
            return NotFound(new { message = "Patient or Staff not found, or doctor has a time conflict." });
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentResponse>> Update(int id, [FromBody] UpdateAppointmentRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateAsync(id, request, ct);
        if (result == null)
            return NotFound(new { message = "Appointment not found, cancelled, or doctor has a time conflict." });
        return Ok(result);
    }

    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentResponse>> Cancel(int id, [FromBody] CancelAppointmentRequest? request, CancellationToken ct)
    {
        var result = await _service.CancelAsync(id, request, ct);
        if (result == null)
            return NotFound(new { message = "Appointment not found or already cancelled." });
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentResponse>> GetById(int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        if (result == null)
            return NotFound(new { message = "Appointment not found." });
        return Ok(result);
    }

    [HttpGet("doctor/{staffId:int}/availability")]
    [ProducesResponseType(typeof(DoctorDayAvailabilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDayAvailabilityResponse>> GetDoctorDayAvailability(int staffId, [FromQuery] DateOnly date, CancellationToken ct)
    {
        var result = await _service.GetDoctorDayAvailabilityAsync(staffId, date, ct);
        if (result == null)
            return NotFound(new { message = "Staff/Doctor not found." });
        return Ok(result);
    }
}
