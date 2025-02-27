using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;
using OperationShuldersAPi.Services;

var builder = WebApplication.CreateBuilder(args);

// ����������� DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� ������������
builder.Services.AddControllers();

// ����������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����������� ������� ��� �������� email
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// ����������� ������� ������ (���� ����������� �� ��� �����������)
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
