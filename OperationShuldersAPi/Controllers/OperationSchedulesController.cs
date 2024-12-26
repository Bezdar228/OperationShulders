using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationSchedulesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OperationSchedulesController(AppDbContext context)
        {
            _context = context;
        }

        // Получить все записи
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _context.OperationSchedules.ToListAsync();
            return Ok(schedules);
        }

        // Создать запись
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] OperationSchedule schedule)
        {
            _context.OperationSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllSchedules), new { id = schedule.Operation_Id }, schedule);
        }

        // Обновить запись
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] OperationSchedule updatedSchedule)
        {
            var schedule = await _context.OperationSchedules.FindAsync(id);
            if (schedule == null) return NotFound("Расписание не найдено.");

            schedule.OperationDate = updatedSchedule.OperationDate;
            schedule.StartTime = updatedSchedule.StartTime;
            schedule.EndTime = updatedSchedule.EndTime;
            schedule.SurgeonId = updatedSchedule.SurgeonId;
            schedule.OperatingRoomId = updatedSchedule.OperatingRoomId;
            schedule.Status = updatedSchedule.Status;

            await _context.SaveChangesAsync();
            return Ok(schedule);
        }

        // Удалить запись
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.OperationSchedules.FindAsync(id);
            if (schedule == null) return NotFound("Расписание не найдено.");

            _context.OperationSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Запись удалена." });
        }
    }

}
