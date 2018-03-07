﻿using System.Security.Cryptography;
using System.Text;

namespace OpenTibia.Security.Encryption
{
    public static class Hash
    {
        private static readonly SHA256 Sha = new SHA256Managed();

        /// <summary>
        /// Encrypts a string using the SHA256 (Secure Hash Algorithm) algorithm.
        /// Details: http://www.itl.nist.gov/fipspubs/fip180-1.htm
        /// This works in the same manner as MD5, providing however 256bit encryption.
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA256 algorithm.</returns>
        public static string Sha256Hash(string data)
        {
            byte[] hash = Sha.ComputeHash(Encoding.Unicode.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
    }
}
