namespace Chat_Backend.Models.Options;

public class DatabaseOptions
{
    public string AccountsCollectionName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}