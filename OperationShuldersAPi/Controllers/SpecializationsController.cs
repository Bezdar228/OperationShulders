using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpecializationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            var specs = await _context.Specializations.ToListAsync();
            return Ok(specs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecializationById(int id)
        {
            var spec = await _context.Specializations.FindAsync(id);
            if (spec == null) return NotFound("Специализация не найдена.");
            return Ok(spec);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialization([FromBody] Specialization spec)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Specializations.Add(spec);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSpecializationById), new { id = spec.SpecializationId }, spec);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialization(int id, [FromBody] Specialization updatedSpec)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var spec = await _context.Specializations.FindAsync(id);
            if (spec == null) return NotFound("Специализация не найдена.");

            spec.SpecializationName = updatedSpec.SpecializationName;

            await _context.SaveChangesAsync();
            return Ok(spec);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var spec = await _context.Specializations.FindAsync(id);
            if (spec == null) return NotFound("Специализация не найдена.");

            _context.Specializations.Remove(spec);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Специализация удалена." });
        }
    }
}
