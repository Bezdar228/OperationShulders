using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OperationShuldersAPi.Models;
using OperationShuldersAPi.Services;

namespace OperationShuldersAPi.Services
{
    public class ScheduleUpdateService : BackgroundService
    {
        private readonly ILogger<ScheduleUpdateService> _logger;
        private readonly IServiceProvider _services;

        public ScheduleUpdateService(ILogger<ScheduleUpdateService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ScheduleUpdateService запущена.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var recentUpdates = await dbContext.OperationSchedules
                            .Where(os => EF.Functions.DateDiffMinute(os.CreatedAt, DateTime.UtcNow) <= 5)
                            .ToListAsync(stoppingToken);

                        if (recentUpdates.Any())
                        {
                            _logger.LogInformation("Обнаружено {Count} обновлений расписания.", recentUpdates.Count);

                            // Получаем сервис отправки email
                            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                            // Отправляем уведомление на указанную почту
                            await emailSender.SendEmailAsync(
                                "e.ovsanik@mail.ru",
                                "Обновление расписания",
                                $"Обнаружено {recentUpdates.Count} обновлений расписания."
                            );
                        }
                        else
                        {
                            _logger.LogInformation("Обновлений расписания не обнаружено.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка в ScheduleUpdateService.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("ScheduleUpdateService остановлена.");
        }
    }
}
