using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptionDecryption
{
    public static class Extensions
    {
        public static byte[] ToByteArrayFromHex(this string hex, int byteCount = 2)
        {
            var sb = new List<byte>();
            for (var i = 0; i < hex.Length; i += byteCount)
            {
                sb.Add(Convert.ToByte(hex.Substring(i, byteCount), 8 * byteCount));
            }

            return sb.ToArray();
        }

        public static string ToHexStringFromByteArray(this IEnumerable<byte> data, int byteCount = 2)
        {
            var format = "{0:x" + byteCount + "}";

            var stringBuilder = new StringBuilder();

            foreach (var theByte in data) stringBuilder.AppendFormat(format, theByte);

            return stringBuilder.ToString();
        }
    }
}