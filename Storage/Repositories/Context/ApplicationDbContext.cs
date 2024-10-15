using Chat_Backend.Models.Backend;
using Microsoft.EntityFrameworkCore;

namespace Chat_Backend.Repositories.Context;

/// <summary>
/// Контекст базы данных для приложения.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Набор данных для работы с аккаунтами
    public DbSet<Account> Accounts { get; set; }

    // Метод для конфигурации модели
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Здесь можно настроить дополнительные параметры модели, например, ограничения, индексы и т.д.
        modelBuilder.Entity<Account>()
            .HasKey(a => a.Id); // Установка первичного ключа
    }
}