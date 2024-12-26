using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperatingRoomsController : Controller
    {
        private readonly AppDbContext _context;

        public OperatingRoomsController(AppDbContext context)
        {
            _context = context;
        }

        // Получить все записи
        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _context.OperatingRooms.ToListAsync();
            return Ok(rooms);
        }
    }
}
