using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionDecryption
{
    public class MD5CryptographyService : ICryptographyService
    {
        #region Implementation

        public string Decrypt(string text)
        {
            throw new NotImplementedException("MD5 only provides one way encryption");
        }

        public string Encrypt(string text)
        {
            var md5CryptoServiceProvider = new MD5CryptoServiceProvider();

            md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(text));

            return md5CryptoServiceProvider.Hash.ToHexStringFromByteArray();
        }

        #endregion
    }
}