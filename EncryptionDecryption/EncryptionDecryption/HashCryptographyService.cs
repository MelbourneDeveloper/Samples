using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionDecryption
{
    public class HashCryptographyService : ICryptographyService
    {
        #region Constructor

        public HashCryptographyService(HashAlgorithm hashAlgorithm)
        {
            HashAlgorithm = hashAlgorithm;
        }

        #endregion

        #region Public Properties

        public HashAlgorithm HashAlgorithm { get; }

        #endregion

        #region Implementation

        public string Decrypt(string text)
        {
            throw new NotImplementedException("Hash algorithms only provide one way encryption");
        }

        public string Encrypt(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);

            HashAlgorithm.ComputeHash(buffer);

            var hash = HashAlgorithm.Hash;

            return hash.ToHexStringFromByteArray();
        }

        #endregion
    }
}