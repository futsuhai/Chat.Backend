using Chat_Backend.Models.Backend;
using Storage.Models.Backend;

namespace Chat_Backend.Models.Frontend;

public class AccountModel
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string City { get; set; }
    public required int Age { get; set; }
    public required string Bio { get; set; }
    public required List<string> SocialMediaUrls { get; set; } = new List<string>();
    public required List<string> Specializations { get; set; } = new List<string>();
    public Tokens? Tokens { get; set; }
}