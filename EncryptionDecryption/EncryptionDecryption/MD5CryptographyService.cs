using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionDecryption
{
    public class MD5CryptographyService : IOneWayEncryptionService
    {
        #region Implementation

        public string Encrypt(string text)
        {
            var md5CryptoServiceProvider = new MD5CryptoServiceProvider();

            md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(text));

            return md5CryptoServiceProvider.Hash.ToHexStringFromByteArray();
        }

        #endregion
    }
}