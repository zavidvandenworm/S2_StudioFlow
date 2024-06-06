using System.Security.Cryptography;
using System.Text;

namespace ApplicationEF;

public static class PasswordHashing
{
    public static string Hash(string input)
    {
        byte[] password = Encoding.UTF8.GetBytes(input);
        byte[] passwordHash = SHA512.HashData(password);

        StringBuilder sb = new StringBuilder();
        foreach (byte b in passwordHash)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}