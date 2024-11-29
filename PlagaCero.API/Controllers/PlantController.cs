using Fition.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlagaCero.API.Models;

namespace PlagaCero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly AppDb _context;

        public PlantController(AppDb context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlantDto>> GetPlant(int id)
        {
            if (id != 1)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.PlantId == 1);

            if (plant == null)
            {
                return NotFound();
            }

            var plantDto = new PlantDto
            {
                PlantId = plant.PlantId,
                FechaRegistro = plant.FechaRegistro,
                // Optional: You can include temperatures and states as well if needed
            };

            return Ok(plantDto);
        }

        [HttpPost]
        public async Task<ActionResult<PlantDto>> PostPlant()
        {
            // Check if the plant with ID 1 already exists
            var existingPlant = await _context.Plants
                .FirstOrDefaultAsync(p => p.PlantId == 1);
            if (existingPlant != null)
            {
                var plantDto = new PlantDto
                {
                    PlantId = existingPlant.PlantId,
                    FechaRegistro = existingPlant.FechaRegistro
                };

                return Ok(plantDto);
            }

            // Otherwise, create the new plant with hardcoded ID 1
            var plant = new Plant
            {
                PlantId = 1,
                FechaRegistro = DateTime.Now // You can adjust the default registration date as needed
            };

            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();

            var plantDtoNew = new PlantDto
            {
                PlantId = plant.PlantId,
                FechaRegistro = plant.FechaRegistro
            };

            return CreatedAtAction(nameof(GetPlant), new { id = plant.PlantId }, plantDtoNew);
        }

        [HttpPost("state")]
        public async Task<ActionResult<PlantStateDto>> PostPlantState([FromBody] PlantStateDto plantStateDto)
        {
            if (plantStateDto == null || !plantStateDto.Fecha.HasValue || string.IsNullOrEmpty(plantStateDto.State))
            {
                return BadRequest("Invalid data.");
            }

            // Check if plant with ID 1 exists, if not create it
            var existingPlant = await _context.Plants
                .FirstOrDefaultAsync(p => p.PlantId == 1);
            if (existingPlant == null)
            {
                existingPlant = new Plant
                {
                    PlantId = 1,
                    FechaRegistro = DateTime.Now
                };
                _context.Plants.Add(existingPlant);
                await _context.SaveChangesAsync();
            }

            var plantState = new PlantState
            {
                Fecha = plantStateDto.Fecha.Value,
                State = plantStateDto.State,
                PlantId = 1 // Always associate with plant ID 1
            };

            _context.PlantStates.Add(plantState);
            await _context.SaveChangesAsync();

            var plantStateDtoCreated = new PlantStateDto
            {
                Fecha = plantState.Fecha,
                State = plantState.State,
                PlantId = plantState.PlantId
            };

            return CreatedAtAction(nameof(GetPlant), new { id = plantState.PlantId }, plantStateDtoCreated);
        }

        [HttpPost("temperature")]
        public async Task<ActionResult<PlantTemperatureDto>> PostPlantTemperature([FromBody] PlantTemperatureDto plantTemperatureDto)
        {
            if (plantTemperatureDto == null || !plantTemperatureDto.Fecha.HasValue || !plantTemperatureDto.Temperatura.HasValue)
            {
                return BadRequest("Invalid data.");
            }

            // Check if plant with ID 1 exists, if not create it
            var existingPlant = await _context.Plants
                .FirstOrDefaultAsync(p => p.PlantId == 1);
            if (existingPlant == null)
            {
                existingPlant = new Plant
                {
                    PlantId = 1,
                    FechaRegistro = DateTime.Now
                };
                _context.Plants.Add(existingPlant);
                await _context.SaveChangesAsync();
            }

            var plantTemperature = new PlantTemperature
            {
                Fecha = plantTemperatureDto.Fecha.Value,
                Temperatura = plantTemperatureDto.Temperatura.Value,
                PlantId = 1 // Always associate with plant ID 1
            };

            _context.PlantTemperatures.Add(plantTemperature);
            await _context.SaveChangesAsync();

            var plantTemperatureDtoCreated = new PlantTemperatureDto
            {
                Fecha = plantTemperature.Fecha,
                Temperatura = plantTemperature.Temperatura,
                PlantId = plantTemperature.PlantId
            };

            return CreatedAtAction(nameof(GetPlant), new { id = plantTemperature.PlantId }, plantTemperatureDtoCreated);
        }
    }
}
