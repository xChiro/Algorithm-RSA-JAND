using System.Numerics;
using System.Security.Cryptography;

namespace JND.Algebra.SRA.Cryptography.PrimesGenerator;

public static class RandomNumber
{
    public static BigInteger Generate(int bitLength)
    {
        var byteLength = (bitLength + 7) / 8;
        var bytes = new byte[byteLength];

        RandomNumberGenerator.Create().GetBytes(bytes);

        var randomNumber = new BigInteger(bytes);
        randomNumber = BigInteger.Abs(randomNumber) | (BigInteger.One << (bitLength - 1));

        return randomNumber;
    }
}