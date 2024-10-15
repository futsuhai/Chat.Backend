using Chat_Backend.Mappers;
using Chat_Backend.Models.Options;
using Chat_Backend.Repositories.AccountRepository;
using Chat_Backend.Repositories.Context;
using Chat_Backend.Services.AccountService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Services.AccountService;

var builder = WebApplication.CreateBuilder(args);

// Добавление служб для Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Репозитории
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// Mapper
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

// Options
builder.Services.AddSingleton<CryptoOptions>();
builder.Services.AddSingleton<JwtOptions>();

// Получение строки подключения из конфигурации
var appConnectionString = builder.Configuration.GetConnectionString("ApplicationDb");

// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(appConnectionString, 
        npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

// Сервисы
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddControllers();

var app = builder.Build();

// Enable Swagger middleware in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Swagger UI available at the root URL
    });
}

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

// Ensure UseRouting is called before other middleware
app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthorization();

// Use top-level route registrations for controllers
app.MapControllers(); // This registers the routes for all controllers

app.Run();