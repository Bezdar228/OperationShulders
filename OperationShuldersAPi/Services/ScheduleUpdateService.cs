using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;
using OperationShuldersAPi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

                        // Пример: проверяем, были ли обновления расписания за последние 5 минут
                        var recentUpdates = await dbContext.OperationSchedules
                            .Where(os => EF.Functions.DateDiffMinute(os.CreatedAt, DateTime.UtcNow) <= 5)
                            .ToListAsync(stoppingToken);

                        if (recentUpdates.Any())
                        {
                            _logger.LogInformation("Обнаружено {Count} обновлений расписания.", recentUpdates.Count);
                            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                            // Отправка уведомления на Gmail
                            await emailSender.SendEmailAsync(
                                "pfxaiznikc@rambler.ru",  // Замените на нужный Gmail адрес
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
