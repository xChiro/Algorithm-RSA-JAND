using JND.Algebra.SRA.Cryptography.PrimesGenerator;
using JND.Algebra.SRA.Tests.Asserts;
using Xunit;

namespace JND.Algebra.SRA.Tests;

public class RsaSecurityTest
{
    [Fact]
    public void Generate_a_large_prime_number_given_a_key_size()
    {
        // Arrange
        const int keySize = 1024;
        
        // Act
        var result = PrimeNumberGenerator.Generate(keySize);

        // Assert
        MillerRobinAssert.IsPrime(result, keySize);
    }
}