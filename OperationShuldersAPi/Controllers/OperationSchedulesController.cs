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

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([Bind("SurgeonId,OperationDate,StartTime,EndTime,OperatingRoomId,Status,CreatedAt")] OperationSchedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращает ошибки валидации
            }

            _context.OperationSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllSchedules), new { id = schedule.Operation_Id }, schedule);
        }



        // Обновить запись
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] OperationSchedule updatedSchedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Ошибки валидации
            }

            var schedule = await _context.OperationSchedules.FindAsync(id);
            if (schedule == null) return NotFound("Расписание не найдено.");

            // Обновляем данные
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

            try
            {
                _context.OperationSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Conflict("Не удалось удалить расписание. Убедитесь, что у него нет зависимостей.");
            }

            return Ok(new { message = "Запись удалена." });
        }

    }

}
