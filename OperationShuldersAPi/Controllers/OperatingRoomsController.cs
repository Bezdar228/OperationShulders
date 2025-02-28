using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperatingRoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OperatingRoomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOperatingRooms()
        {
            var rooms = await _context.OperatingRooms.ToListAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOperatingRoomById(int id)
        {
            var room = await _context.OperatingRooms.FindAsync(id);
            if (room == null) return NotFound("Операционная не найдена.");
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOperatingRoom([FromBody] OperatingRoom room)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.OperatingRooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOperatingRoomById), new { id = room.RoomId }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOperatingRoom(int id, [FromBody] OperatingRoom updatedRoom)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var room = await _context.OperatingRooms.FindAsync(id);
            if (room == null) return NotFound("Операционная не найдена.");

            room.RoomName = updatedRoom.RoomName;
            room.Capacity = updatedRoom.Capacity;
            room.EquipmentDetails = updatedRoom.EquipmentDetails;

            await _context.SaveChangesAsync();
            return Ok(room);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperatingRoom(int id)
        {
            var room = await _context.OperatingRooms.FindAsync(id);
            if (room == null) return NotFound("Операционная не найдена.");

            _context.OperatingRooms.Remove(room);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Операционная удалена." });
        }
    }
}
