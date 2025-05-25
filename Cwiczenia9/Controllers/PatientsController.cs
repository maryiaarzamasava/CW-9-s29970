using Cwiczenia9.Exceptions;
using Cwiczenia9.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia9.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController(IDbService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        try
        {
            return Ok(await service.GetPatientDetailsByIdAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}