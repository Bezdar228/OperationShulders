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
            // ������� in-memory ���� ������ ��� ������
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_xUnit")
                .Options;
            _context = new AppDbContext(options);

            // �������������� IMemoryCache
            _cache = new MemoryCache(new MemoryCacheOptions());

            // ��������� ���������� ����������� ������ ��� ������� ������������:
            // ��������������, ��� � ������ ������������: Specialization, Users, Surgeon, OperatingRoom
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

            // ������� ��������� ����������� � �������������
            _controller = new OperationSchedulesController(_context, _cache);
        }

        [Fact]
        public async Task GetAllSchedules_ReturnsOkResult_WithListOfSchedules()
        {
            // Arrange: ��������� �������� ������
            var schedule = new OperationSchedule
            {
                SurgeonId = 3,
                OperationDate = DateTime.UtcNow.AddDays(1),
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                OperatingRoomId = 1,
                Status = "�������������",
                CreatedAt = DateTime.UtcNow
            };
            _context.OperationSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            // Act: �������� ����� GET
            var result = await _controller.GetAllSchedules();

            // Assert: ��������� ������ ���� OkObjectResult � �������� �������
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<System.Collections.Generic.List<OperationSchedule>>(okResult.Value);
            Assert.NotEmpty(list);

            // ���������, ��� ������ ����������
            Assert.True(_cache.TryGetValue("AllSchedules", out _));
        }

        [Fact]
        public async Task CreateSchedule_WithValidData_ReturnsCreatedAtActionResult_AndClearsCache()
        {
            // Arrange: ������� ������ ���������� � ����������� �������
            var schedule = new OperationSchedule
            {
                SurgeonId = 3,
                OperationDate = DateTime.UtcNow.AddDays(2),
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(12),
                OperatingRoomId = 1,
                Status = "�������������",
                CreatedAt = DateTime.UtcNow
            };

            // �������������� ��������� ��� ������� GET
            await _controller.GetAllSchedules();
            Assert.True(_cache.TryGetValue("AllSchedules", out _));

            // Act: �������� ����� POST ��� �������� ������
            var result = await _controller.CreateSchedule(schedule);

            // Assert: ���������, ��� ��������� ����� ��� CreatedAtActionResult
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult.Value);

            // ���������, ��� ��� ������ ����� �������� ������
            Assert.False(_cache.TryGetValue("AllSchedules", out _));
        }

        public void Dispose()
        {
            // ������� �������� ����� ������� �����
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _cache.Dispose();
        }
    }
}
