using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace EncryptionDecryption
{
    /// <summary>
    ///     Modified from
    ///     https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netframework-4.8
    /// </summary>
    public class SymmetricAlgorithmCryptographyService : ICryptographyService, IDisposable
    {
        #region Public Properties

        public SymmetricAlgorithm SymmetricAlgorithm { get; }

        #endregion

        #region Constructors

        public SymmetricAlgorithmCryptographyService(SymmetricAlgorithm symmetricAlgorithm)
        {
            SymmetricAlgorithm = symmetricAlgorithm ?? throw new ArgumentNullException(nameof(SymmetricAlgorithm));

            if (symmetricAlgorithm.Key == null || symmetricAlgorithm.Key.Length <= 0)
                throw new ArgumentNullException(nameof(SymmetricAlgorithm.Key));

            SymmetricAlgorithm = symmetricAlgorithm;
        }

        #endregion

        #region Implementation
        public string Decrypt(string text)
        {
            var cipherTextData = text.ToByteArrayFromHex();
            var hex = DecryptStringFromBytes(cipherTextData);
            return hex;
        }

        public string Encrypt(string text)
        {
            return EncryptStringToBytes(text).ToHexStringFromByteArray();
        }

        public void Dispose()
        {
            SymmetricAlgorithm.Dispose();
        }
        #endregion

        #region Private Methods
        private IEnumerable<byte> EncryptStringToBytes(string plainText)
        {
            if (plainText == null || plainText.Length <= 0) throw new ArgumentNullException(nameof(plainText));

            var cryptoTransform = SymmetricAlgorithm.CreateEncryptor(SymmetricAlgorithm.Key, SymmetricAlgorithm.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, cryptoTransform, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        private string DecryptStringFromBytes(byte[] cipherTextData)
        {
            if (cipherTextData == null || cipherTextData.Length <= 0) throw new ArgumentNullException(nameof(cipherTextData));

            var cryptoTransform = SymmetricAlgorithm.CreateDecryptor(SymmetricAlgorithm.Key, SymmetricAlgorithm.IV);

            using (var msDecrypt = new MemoryStream(cipherTextData))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, cryptoTransform, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        #endregion
    }
}