using Drivers.Api.DriverServices; // Espacio de nombres para acceder a las clases de DriverServices
using Drivers.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Drivers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly DriverServices.DriverServices _driverServices; // Accede a la clase dentro del espacio de nombres

    public DriversController(ILogger<DriversController> logger,
                             DriverServices.DriverServices driverServices) // Especifica la ruta completa
    {
        _logger = logger;
        _driverServices = driverServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetDrivers()
    {
        var drivers = await _driverServices.GetAsync();
        return Ok(drivers);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetDriversById(string Id)
    {
        return Ok(await _driverServices.GetDriverById(Id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] Drive drive)
    {
        if (drive == null)
            return BadRequest();
            if(drive.Nombre == string.Empty)
            ModelState.AddModelError("Nombre", "El driver no debe estar vacio");

            await _driverServices.InsertDriver(drive);

            return Created("Created", true);
    }
    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateDriver([FromBody] Drive driver, string Id)
    {
        if (driver == null)
        return BadRequest();
        if (driver.Nombre == string.Empty)
        ModelState.AddModelError("Nombre", "El driver no debe estar vacio");
        driver.Id = Id;

        await _driverServices.UpdateDriver(driver);
        return Created("Created", true);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteDriver(string Id)
    {
        await _driverServices.DeleteDriver(Id);
        return NoContent();
    }
}
