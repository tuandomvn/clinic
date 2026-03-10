using Microsoft.AspNetCore.Mvc;
using Clinic.Services.Models.Prescription;
using Clinic.Services.Services.Prescriptions;

namespace Clinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PrescriptionManagementController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionManagementController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionResponse>> Create([FromBody] CreatePrescriptionRequest request, CancellationToken ct)
    {
        var (result, error) = await _service.CreateAsync(request, ct);
        if (error != null)
            return BadRequest(new { message = error });
        if (result == null)
            return NotFound(new { message = "HealthRecord not found." });
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionResponse>> GetById(int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        if (result == null)
            return NotFound(new { message = "Prescription not found." });
        return Ok(result);
    }

    [HttpGet("by-health-record/{healthRecordId:int}")]
    [ProducesResponseType(typeof(List<PrescriptionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PrescriptionResponse>>> GetByHealthRecord(int healthRecordId, CancellationToken ct)
    {
        var result = await _service.GetByHealthRecordIdAsync(healthRecordId, ct);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionResponse>> Update(int id, [FromBody] UpdatePrescriptionRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateAsync(id, request, ct);
        if (result == null)
            return NotFound(new { message = "Prescription not found." });
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound(new { message = "Prescription not found." });
        return NoContent();
    }
}
