using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using OperationShuldersAPi.Controllers;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Tests
{
    public class OperationSchedulesControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly OperationSchedulesController _controller;
        private readonly IMemoryCache _cache;

        public OperationSchedulesControllerTests()
        {
            // Создаем in-memory базу данных для тестов
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_xUnit")
                .Options;
            _context = new AppDbContext(options);

            // Инициализируем IMemoryCache
            _cache = new MemoryCache(new MemoryCacheOptions());

            // Добавляем минимально необходимые данные для внешних зависимостей:
            // Предполагается, что в модели присутствуют: Specialization, Users, Surgeon, OperatingRoom
            _context.Specializations.Add(new Specialization
            {
                SpecializationId = 1,
                SpecializationName = "Test Specialization"
            });
            _context.Users.Add(new Users
            {
                User_Id = 1,
                Username = "TestUser",
                Password = "pass",
                Role = "TestRole",
                Created_At = DateTime.UtcNow
            });
            _context.Surgeons.Add(new Surgeon
            {
                SurgeonId = 3,
                UserId = 1,
                FullName = "Test Surgeon",
                SpecializationId = 1,
                ContactInfo = "test@example.com",
                Active = true,
                CreatedAt = DateTime.UtcNow
            });
            _context.OperatingRooms.Add(new OperatingRoom
            {
                RoomId = 1,
                RoomName = "Test OR",
                Capacity = 5,
                EquipmentDetails = "Test Equipment"
            });
            _context.SaveChanges();

            // Создаем экземпляр контроллера с зависимостями
            _controller = new OperationSchedulesController(_context, _cache);
        }

        [Fact]
        public async Task GetAllSchedules_ReturnsOkResult_WithListOfSchedules()
        {
            // Arrange: добавляем тестовую запись
            var schedule = new OperationSchedule
            {
                SurgeonId = 3,
                OperationDate = DateTime.UtcNow.AddDays(1),
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                OperatingRoomId = 1,
                Status = "Запланировано",
                CreatedAt = DateTime.UtcNow
            };
            _context.OperationSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            // Act: вызываем метод GET
            var result = await _controller.GetAllSchedules();

            // Assert: результат должен быть OkObjectResult с непустым списком
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<System.Collections.Generic.List<OperationSchedule>>(okResult.Value);
            Assert.NotEmpty(list);

            // Проверяем, что данные кэшируются
            Assert.True(_cache.TryGetValue("AllSchedules", out _));
        }

        [Fact]
        public async Task CreateSchedule_WithValidData_ReturnsCreatedAtActionResult_AndClearsCache()
        {
            // Arrange: создаем объект расписания с корректными данными
            var schedule = new OperationSchedule
            {
                SurgeonId = 3,
                OperationDate = DateTime.UtcNow.AddDays(2),
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(12),
                OperatingRoomId = 1,
                Status = "Запланировано",
                CreatedAt = DateTime.UtcNow
            };

            // Предварительно загружаем кэш вызовом GET
            await _controller.GetAllSchedules();
            Assert.True(_cache.TryGetValue("AllSchedules", out _));

            // Act: вызываем метод POST для создания записи
            var result = await _controller.CreateSchedule(schedule);

            // Assert: проверяем, что результат имеет тип CreatedAtActionResult
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult.Value);

            // Проверяем, что кэш очищен после создания записи
            Assert.False(_cache.TryGetValue("AllSchedules", out _));
        }

        public void Dispose()
        {
            // Очистка ресурсов после каждого теста
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _cache.Dispose();
        }
    }
}
