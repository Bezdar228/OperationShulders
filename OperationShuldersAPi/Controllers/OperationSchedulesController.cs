using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationSchedulesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private const string SchedulesCacheKey = "AllSchedules";

        public OperationSchedulesController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Получить все записи с использованием кэша
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            // Попытка получить данные из кэша
            if (!_cache.TryGetValue(SchedulesCacheKey, out List<OperationSchedule> schedules))
            {
                // Оптимизация запроса: если данные только для чтения, используйте AsNoTracking()
                schedules = await _context.OperationSchedules.AsNoTracking().ToListAsync();

                // Настройка параметров кэширования (например, кэширование на 5 минут)
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                // Сохраняем данные в кэш
                _cache.Set(SchedulesCacheKey, schedules, cacheEntryOptions);
            }

            return Ok(schedules);
        }

        // Создание записи (после успешного создания сбрасываем кэш)
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] OperationSchedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.OperationSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            // Очистка кэша, чтобы новые данные были подгружены
            _cache.Remove(SchedulesCacheKey);

            return CreatedAtAction(nameof(GetAllSchedules), new { id = schedule.Operation_Id }, schedule);
        }

        // Обновление записи (после обновления сбрасываем кэш)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] OperationSchedule updatedSchedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schedule = await _context.OperationSchedules.FindAsync(id);
            if (schedule == null) return NotFound("Расписание не найдено.");

            schedule.OperationDate = updatedSchedule.OperationDate;
            schedule.StartTime = updatedSchedule.StartTime;
            schedule.EndTime = updatedSchedule.EndTime;
            schedule.SurgeonId = updatedSchedule.SurgeonId;
            schedule.OperatingRoomId = updatedSchedule.OperatingRoomId;
            schedule.Status = updatedSchedule.Status;

            await _context.SaveChangesAsync();

            // Очистка кэша после обновления записи
            _cache.Remove(SchedulesCacheKey);

            return Ok(schedule);
        }

        // Удаление записи (также сбрасываем кэш)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.OperationSchedules.FindAsync(id);
            if (schedule == null) return NotFound("Расписание не найдено.");

            _context.OperationSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            // Очистка кэша после удаления
            _cache.Remove(SchedulesCacheKey);

            return Ok(new { message = "Запись удалена." });
        }
    }
}
