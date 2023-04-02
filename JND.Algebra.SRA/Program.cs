// See https://aka.ms/new-console-template for more information

using System.Numerics;
using JND.Algebra.SRA.Cryptography;

namespace JND.Algebra.SRA;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Bienvenido a la aplicación de cifrado RSA");

        Init:
        Console.WriteLine("Introduzca el tamaño de la clave");
        var keySizeString = Console.ReadLine();

        if (!int.TryParse(keySizeString, out var keySize))
        {
            Console.WriteLine("El tamaño de la clave debe ser un número entero");
            goto Init;
        }

        Console.WriteLine("Conoce las claves publicas, privadas y el numero PublicExponent?(S/N)");
        var knowKeys = Console.ReadLine();
        
        RSACrypto rsaCryptography;

        if (knowKeys == "S")
        {
            Console.WriteLine("Introduzca el valor de PublicExponent");
            var n = Console.ReadLine();
            Console.WriteLine("Introduzca el valor de la clave publica");
            var publicKey = Console.ReadLine();
            Console.WriteLine("Introduzca el valor de la clave privada");
            var privateKey = Console.ReadLine();

            if (!BigInteger.TryParse(n, out var nValue) || !BigInteger.TryParse(publicKey, out var publicKeyValue) ||
                !BigInteger.TryParse(privateKey, out var privateKeyValue))
            {
                Console.WriteLine("Los valores introducidos no son correctos");
                goto Init;
            }

            rsaCryptography = new RSACrypto(keySize, publicKeyValue, privateKeyValue, nValue);
        }
        else
        {
            rsaCryptography = new RSACrypto(keySize);
        }

        var closeApplication = false;

        do
        {
            Console.WriteLine("Que desea hacer?");
            Console.WriteLine("1. Encriptar");
            Console.WriteLine("2. Desencriptar");
            Console.WriteLine("3. Mostrar llaves (información para desencriptar, mantener en secreto)");
            Console.WriteLine("4. Salir");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.WriteLine("Introduzca el mensaje a encriptar");
                    var message = Console.ReadLine();
                    var encryptedMessage = rsaCryptography.Encrypt(message ?? "Hello World");
                    Console.WriteLine($"El mensaje encriptado es: {encryptedMessage}");
                    break;
                case "2":
                    Console.WriteLine("Introduzca el mensaje a desencriptar");
                    var encryptedMessageToDecrypt = Console.ReadLine();
                    var decryptedMessage = rsaCryptography.Decrypt(encryptedMessageToDecrypt ?? "Hello World");
                    Console.WriteLine($"El mensaje desencriptado es: {decryptedMessage}");
                    break;
                case "3":
                    Console.WriteLine($"Llave exponente publico {rsaCryptography.PublicExponent}");
                    Console.WriteLine($"Llave publica {rsaCryptography.PublicKey}");
                    Console.WriteLine($"Llave privada {rsaCryptography.GetPrivateKey()}");
                    break;
                case "4":
                    closeApplication = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida");
                    break;
            }
        } while (!closeApplication);
    }
}