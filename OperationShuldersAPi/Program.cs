using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;
using OperationShuldersAPi.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрация DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация контроллеров
builder.Services.AddControllers();

// Регистрация Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация сервиса для отправки email
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// Регистрация фоновой службы (если используете ее для уведомлений)
builder.Services.AddHostedService<OperationShuldersAPi.Services.ScheduleUpdateService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
