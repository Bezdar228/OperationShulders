using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurgeonsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SurgeonsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSurgeons()
        {
            var surgeons = await _context.Surgeons.ToListAsync();
            return Ok(surgeons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurgeonById(int id)
        {
            var surgeon = await _context.Surgeons.FindAsync(id);
            if (surgeon == null) return NotFound("Хирург не найден.");
            return Ok(surgeon);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurgeon([FromBody] Surgeon surgeon)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Surgeons.Add(surgeon);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSurgeonById), new { id = surgeon.SurgeonId }, surgeon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSurgeon(int id, [FromBody] Surgeon updatedSurgeon)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var surgeon = await _context.Surgeons.FindAsync(id);
            if (surgeon == null) return NotFound("Хирург не найден.");

            surgeon.FullName = updatedSurgeon.FullName;
            surgeon.SpecializationId = updatedSurgeon.SpecializationId;
            surgeon.ContactInfo = updatedSurgeon.ContactInfo;
            surgeon.Active = updatedSurgeon.Active;

            await _context.SaveChangesAsync();
            return Ok(surgeon);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurgeon(int id)
        {
            var surgeon = await _context.Surgeons.FindAsync(id);
            if (surgeon == null) return NotFound("Хирург не найден.");

            _context.Surgeons.Remove(surgeon);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Хирург удалён." });
        }
    }
}
