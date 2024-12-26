using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Добавление сервисов в контейнер
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Добавление контроллеров
            builder.Services.AddControllers();

            // Добавление Swagger для API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Конфигурация HTTP запроса
            if (app.Environment.IsDevelopment())
            {
                // Включение Swagger UI в режиме разработки
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    // По умолчанию Swagger будет доступен по адресу /swagger
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
