using Cwiczenia9.DTOs;
using Cwiczenia9.Exceptions;
using Cwiczenia9.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia9.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptionsController(IDbService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescriptionDetails([FromRoute] int id)
    {
        try
        {
            return Ok(await service.GetPrescriptionByIdAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionCreateDto prescriptionData)
    {
        try
        {
            var prescription = await service.CreatePrescriptionAsync(prescriptionData);
            return CreatedAtAction(nameof(GetPrescriptionDetails), new { id = prescription.IdPrescription },
                prescription);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}