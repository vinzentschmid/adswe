using System.Security.Cryptography;
using System.Text;

namespace DAL.Utils;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 Bit
    private const int KeySize = 32; // 256 Bit
    private const int Iterations = 100000; // Mindestens 100.000 Iterationen
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[SaltSize];
        rng.GetBytes(salt);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithm, KeySize);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Check(string hash, string password)
    {
        var parts = hash.Split('.');
        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] storedHash = Convert.FromBase64String(parts[1]);

        byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithm, KeySize);
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}