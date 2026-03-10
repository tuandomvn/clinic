using Microsoft.AspNetCore.Mvc;
using Clinic.Services.Models.Surgery;
using Clinic.Services.Services.Surgeries;

namespace Clinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SurgeryScheduleManagementController : ControllerBase
{
    private readonly ISurgeryScheduleService _service;

    public SurgeryScheduleManagementController(ISurgeryScheduleService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SurgeryScheduleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurgeryScheduleResponse>> Create([FromBody] CreateSurgeryScheduleRequest request, CancellationToken ct)
    {
        var (result, error) = await _service.CreateAsync(request, ct);
        if (error != null)
            return BadRequest(new { message = error });
        if (result == null)
            return NotFound(new { message = "Patient or staff not found." });
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SurgeryScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurgeryScheduleResponse>> GetById(int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        if (result == null)
            return NotFound(new { message = "Surgery schedule not found." });
        return Ok(result);
    }

    [HttpGet("by-date")]
    [ProducesResponseType(typeof(List<SurgeryScheduleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SurgeryScheduleResponse>>> GetByDate([FromQuery] DateOnly date, CancellationToken ct)
    {
        var result = await _service.GetByDateAsync(date, ct);
        return Ok(result);
    }
}
