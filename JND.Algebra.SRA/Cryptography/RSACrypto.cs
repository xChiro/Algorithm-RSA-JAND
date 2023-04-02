using System.Numerics;
using System.Text;
using JND.Algebra.SRA.Cryptography.PrimesGenerator;

namespace JND.Algebra.SRA.Cryptography;

public class RSACrypto
{
    private readonly int _keySize;
    private readonly BigInteger _privateKey;

    public RSACrypto(int keySize)
    {
        _keySize = keySize;

        var keys = GenerateKeys();
        PublicExponent = keys.publicExponent;
        PublicKey = keys.publicKey;
        _privateKey = keys.privateKey;
    }

    public RSACrypto(int keySize, BigInteger publicKey, BigInteger privateKey, BigInteger publicExponent)
    {
        _keySize = keySize;
        PublicExponent = publicExponent;
        PublicKey = publicKey;
        _privateKey = privateKey;
    }

    public BigInteger PublicExponent { get; }
    public BigInteger PublicKey { get; }

    private (BigInteger publicExponent, BigInteger publicKey, BigInteger privateKey) GenerateKeys()
    {
        var p = PrimeNumberGenerator.Generate(_keySize / 2);
        var q = PrimeNumberGenerator.Generate(_keySize / 2);
        var n = p * q;
        var phi = (p - 1) * (q - 1);

        var e = FindCoprime(phi);
        var d = FindModularInverse(e, phi);

        return (n, e, d);
    }

    private BigInteger FindModularInverse(BigInteger a, BigInteger m)
    {
        var y = RandomNumber.Generate(_keySize / 2);
        var gcd = ExtendedEuclideanAlgorithm(a, m, out var x, out y);

        if (gcd != BigInteger.One)
        {
            throw new InvalidOperationException("Numbers a and m are not coprime.");
        }

        return (x % m + m) % m;
    }

    private BigInteger FindCoprime(BigInteger phi)
    {
        var e = RandomNumber.Generate(_keySize / 2);

        while (true)
        {
            if (BigInteger.GreatestCommonDivisor(e, phi) == BigInteger.One)
            {
                return e;
            }
            
            e += BigInteger.One;
        }
    }

    private static BigInteger ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        if (b == BigInteger.Zero)
        {
            x = BigInteger.One;
            y = BigInteger.Zero;
            return a;
        }

        BigInteger x1, y1;
        var gcd = ExtendedEuclideanAlgorithm(b, a % b, out x1, out y1);
        x = y1;
        y = x1 - (a / b) * y1;

        return gcd;
    }

    public string Encrypt(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var m = new BigInteger(bytes);

        var c = BigInteger.ModPow(m, PublicKey, PublicExponent);
        return Convert.ToBase64String(c.ToByteArray());
    }

    public string Decrypt(string encryptedMessage)
    {
        var bytes = Convert.FromBase64String(encryptedMessage);
        var c = new BigInteger(bytes);

        var m = BigInteger.ModPow(c, _privateKey, PublicExponent);
        return Encoding.UTF8.GetString(m.ToByteArray());
    }

    internal BigInteger GetPrivateKey()
    {
        return _privateKey;
    }
}