using System.Security.Cryptography;
using System.Text;

namespace InfrastructureDapper.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        var byteArray = SHA512.HashData(Encoding.UTF8.GetBytes(password));
        var passwordHashed = Convert.ToHexString(byteArray);

        return passwordHashed;
    }

    public static bool Match(string suppliedPassword, string hash)
    {
        var suppliedPasswordHash = HashPassword(suppliedPassword);
        return string.Equals(hash, suppliedPasswordHash);
    }
}