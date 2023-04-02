using System.Numerics;
using System.Security.Cryptography;
using Xunit;

namespace JND.Algebra.SRA.Tests.Asserts;

public static class MillerRobinAssert
{
    public static void IsPrime(BigInteger number, int certainty = 100)
    {
        var userMessage = $"{number} is not prime";
        Assert.True(Execute(number, certainty), userMessage);
    }

    private static bool Execute(BigInteger number, int certainty)
    {
        if(number == 2 || number == 3)
            return true;
        if(number < 2 || number % 2 == 0)
            return false;

        var d = number - 1;
        var s = 0;

        while(d % 2 == 0)
        {
            d /= 2;
            s += 1;
        }

        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[number.ToByteArray().LongLength];

        for(var i = 0; i < certainty; i++)
        {
            BigInteger a;
            
            do
            {
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while(a < 2 || a >= number - 2);

            var x = BigInteger.ModPow(a, d, number);
            if(x == 1 || x == number - 1)
                continue;

            for(var r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, number);
                if(x == 1)
                    return false;
                if(x == number - 1)
                    break;
            }

            if(x != number - 1)
                return false;
        }

        return true;
    }
}