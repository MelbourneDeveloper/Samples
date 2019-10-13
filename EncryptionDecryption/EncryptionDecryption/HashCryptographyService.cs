using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionDecryption
{
    public class HashCryptographyService : IOneWayEncryptionService
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