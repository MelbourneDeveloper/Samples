using EncryptionDecryptionConsole;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionDecryption
{
    internal class Program
    {
        private const string PlainText = "Test";

        private static IOneWayEncryptionService OneWayEncrpytionService { get; set; }
        private static ICryptographyService CryptographyService { get; set; }

        private static void Main(string[] args)
        {
            RunAsync().Wait();

            Console.ReadLine();
        }

        private async static Task RunAsync()
        {
            Console.WriteLine($"Plain Text: {PlainText}");

            OneWayEncrpytionService = new MD5CryptographyService();
            var hash = OneWayEncrpytionService.Encrypt(PlainText);
            Console.WriteLine($"MD5 Hashed: {hash}");

            OneWayEncrpytionService = new HashCryptographyService(new SHA1Managed());
            hash = OneWayEncrpytionService.Encrypt(PlainText);
            Console.WriteLine($"SHA1 Hashed: {hash}");

            var symmetricAlgorithmCryptographyService =
            new SymmetricAlgorithmCryptographyService(
                new AesCryptoServiceProvider
                {
                    Mode = CipherMode.CBC,
                    KeySize = 128,
                    Key = Encoding.UTF8.GetBytes("1234567890123456"),
                    IV = Encoding.UTF8.GetBytes("0234567890123456")
                });

            CryptographyService = symmetricAlgorithmCryptographyService;
            var encryptedText = CryptographyService.Encrypt(PlainText);
            Console.WriteLine($"AES Encrypted: {encryptedText}");

            var decryptedText = CryptographyService.Decrypt(encryptedText).Trim();
            Console.WriteLine($"AES Decrypted: {decryptedText}");

            const string filename = "Credentials.txt";
            var encryptedPersister = new EncryptedPersister(CryptographyService, new FileIOService());
            await encryptedPersister.SaveAsync(filename, new Credentials { Username = "Bob", Password = "Password123" });
            var credentials = await encryptedPersister.LoadAsync<Credentials>(filename);

            Console.WriteLine($"AES Encrypted File: {await File.ReadAllTextAsync(filename)}");
            Console.WriteLine($"AES Decrypted Credentials Username: {credentials.Username} Password: {credentials.Password}");
        }
    }
}