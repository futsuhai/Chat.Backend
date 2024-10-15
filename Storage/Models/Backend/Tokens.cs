namespace Storage.Models.Backend;

public class Tokens
{
    public int Id { get; set; } // Или другое подходящее имя и тип для первичного ключа
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    // Добавьте другие свойства по мере необходимости
}