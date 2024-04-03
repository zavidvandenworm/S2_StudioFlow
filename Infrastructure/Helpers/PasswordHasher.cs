using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        var byteArray = SHA512.HashData(Encoding.UTF8.GetBytes(password));
        var passwordHashed = Convert.ToHexString(byteArray);

        return passwordHashed;
    }
}