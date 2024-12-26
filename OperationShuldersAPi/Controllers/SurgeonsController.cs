using Microsoft.AspNetCore.Http;
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

        // Получить список всех хирургов
        [HttpGet]
        public async Task<IActionResult> GetAllSurgeons()
        {
            var surgeon = await _context.Surgeons.ToListAsync();
            return Ok(surgeon);
        }
    }

}
