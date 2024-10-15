using System.Security.Cryptography;

namespace Chat_Backend.Models.Options;

public class CryptoOptions
{
    private readonly int byteCount = 32;
    private readonly int iterationCount = 10000;

    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[byteCount];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
    public byte[] GenerateHashPassword(string password, byte[] salt)
    {
        using Rfc2898DeriveBytes pbkdf2 = new(password, salt, iterationCount, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(byteCount);
    }
}