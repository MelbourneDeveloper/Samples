using EncryptionDecryption;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionDecryptionTests
{
    public class AESEncryptionTestsBase
    {
        /// <summary>
        ///     Create the service with the below AES settings
        /// </summary>
        protected readonly SymmetricAlgorithmCryptographyService _symmetricAlgorithmCryptographyService =
            new SymmetricAlgorithmCryptographyService(
                new AesCryptoServiceProvider
                {
                    Mode = CipherMode.CBC,
                    KeySize = 128,
                    Key = Encoding.UTF8.GetBytes(_key),
                    IV = Encoding.UTF8.GetBytes(_iv)
                });

        private const string _key = "1234567890123456";
        private const string _iv = "0234567890123456";
    }
}
