using System.Security.Cryptography;
using EncryptionDecryption;
using NUnit.Framework;

namespace EncryptionDecryptionTests
{
    public class EncryptionTests : AESEncryptionTestsBase
    {
        #region Fields

        /// <summary>
        ///     https://passwordsgenerator.net/sha1-hash-generator/
        /// </summary>
        private const string _sha1EncryptedText = "640AB2BAE07BEDC4C163F679A746F7AB7FB5D1FA";

        /// <summary>
        ///     Test Results Generated Here: https://www.devglan.com/online-tools/aes-encryption-decryption
        ///     Test -> Cipher Mode -> KeySize -> IV -> Key -> Output = Hex
        /// </summary>
        private const string _aesEncryptedText = "C631B7EC0B49774A8394C57E76936304";

        /// <summary>
        ///     Test Results Generated Here: https://www.browserling.com/tools/all-hashes
        /// </summary>
        private const string _md5EncryptedText = "0cbc6611f5540bd0809a388dc95a615b";

        private const string _plaintText = "Test";
        #endregion

        #region Tests

        [Test]
        public void TestMD5()
        {
            var encryptedText = new MD5CryptographyService().Encrypt(_plaintText);

            //Case is irrelevant because the text represents hex
            Assert.AreEqual(_md5EncryptedText.ToLower(), encryptedText.ToLower());
        }

        [Test]
        public void TestSHA1()
        {
            var encryptedText = new HashCryptographyService(new SHA1Managed()).Encrypt(_plaintText);

            //Case is irrelevant because the text represents hex
            Assert.AreEqual(_sha1EncryptedText.ToLower(), encryptedText.ToLower());
        }

        [Test]
        public void TestEncryptAES()
        {
            var encryptedText = _symmetricAlgorithmCryptographyService.Encrypt(_plaintText);

            //Case is irrelevant because the text represents hex
            Assert.AreEqual(_aesEncryptedText.ToLower(), encryptedText.ToLower());
        }

        [Test]
        public void TestDecryptAES()
        {
            var decryptedText = _symmetricAlgorithmCryptographyService.Decrypt(_aesEncryptedText);

            //Remove padding
            var actual = decryptedText.Trim();

            Assert.AreEqual(_plaintText, actual);
        }

        #endregion
    }
}