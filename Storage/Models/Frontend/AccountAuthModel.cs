namespace Chat_Backend.Models.Frontend;

public class AccountAuthModel
{
    public string Login { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? City { get; set; }
    public int? Age { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public List<string>? SocialMediaUrls { get; set; }
    public List<string>? Specializations { get; set; }
}