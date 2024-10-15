using Chat_Backend.Models.Frontend;
using Storage.Models.Backend;

namespace Chat_Backend.Models.Backend;

public class Account
{
    public Guid Id { get; set; }
    public string HashPassword { get; set; }
    public string Salt { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string City { get; set; }
    public int Age { get; set; }
    public string Bio { get; set; }
    public List<string> SocialMediaUrls { get; set; } = new List<string>();
    public List<string> Specializations { get; set; } = new List<string>();
    public Tokens Tokens { get; set; }

    // Пустой конструктор
    public Account() 
    {
        // Инициализация коллекций для предотвращения NullReferenceException
        SocialMediaUrls = new List<string>();
        Specializations = new List<string>();
    }

    // Конструктор с параметрами
    public Account(AccountAuthModel accountAuthModel, string salt, string hashPassword, Tokens tokens)
    {
        Id = Guid.NewGuid(); // Генерируем новый идентификатор
        Salt = salt;
        HashPassword = hashPassword;
        Login = accountAuthModel.Login;
        Email = accountAuthModel.Email ?? string.Empty;
        Name = accountAuthModel.Name ?? string.Empty; // Исправлено: здесь должно быть Name
        Surname = accountAuthModel.Surname ?? string.Empty;
        City = accountAuthModel.City ?? string.Empty;
        Age = accountAuthModel.Age ?? 0;
        Bio = accountAuthModel.Bio ?? string.Empty;
        SocialMediaUrls = accountAuthModel.SocialMediaUrls ?? new List<string>();
        Specializations = accountAuthModel.Specializations ?? new List<string>();
        Tokens = tokens;
    }
}