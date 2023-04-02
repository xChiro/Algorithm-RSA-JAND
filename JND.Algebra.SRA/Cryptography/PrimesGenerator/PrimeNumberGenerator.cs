using System.Numerics;

namespace JND.Algebra.SRA.Cryptography.PrimesGenerator;

public class PrimeNumberGenerator
{
    public static BigInteger Generate(int keySize)
    {
        BigInteger possiblePrime;

        do
        {
            possiblePrime = RandomNumber.Generate(keySize);
            
        }
        while (!MillerRobinPrimalityTest.IsPrime(possiblePrime));
        
        return possiblePrime;
    }
}