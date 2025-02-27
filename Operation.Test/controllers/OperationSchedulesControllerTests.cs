using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;
using OperationShuldersAPi.Controllers;
using System;

public class OperationSchedulesControllerTests
{
    private DbContextOptions<AppDbContext> _dbContextOptions;

    public OperationSchedulesControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task CreateSchedule_WithNullModel_ReturnsBadRequest()
    {
        // Arrange
        var context = new AppDbContext(_dbContextOptions);
        var controller = new OperationSchedulesController(context);

        // Act
        var result = await controller.CreateSchedule(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateSchedule_WithValidData_ReturnsCreated()
    {
        // Arrange
        var context = new AppDbContext(_dbContextOptions);
        var controller = new OperationSchedulesController(context);

        var validSchedule = new OperationSchedule
        {
            SurgeonId = 1,
            OperationDate = DateTime.UtcNow.AddDays(1),
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(11),
            OperatingRoomId = 2,
            Status = "Запланировано"
        };

        // Act
        var result = await controller.CreateSchedule(validSchedule);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }
}
