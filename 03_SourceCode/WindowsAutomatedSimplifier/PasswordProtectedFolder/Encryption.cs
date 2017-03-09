using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WindowsAutomatedSimplifier.PasswordProtectedFolder
{
    internal class Encryption
    {
        /// <summary>
        ///     Create and initialize a crypto algorithm.
        /// </summary>
        /// <param name="password">The password.</param>
        private static SymmetricAlgorithm GetAlgorithm(string password)
        {
            using (Rijndael algorithm = Rijndael.Create())
            {
                Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(password, new byte[]
                {
                    0x53, 0x6f, 0x64, 0x69, 0x75, 0x6d, 0x20, 0x43, 0x68, 0x6c, 0x6f, 0x72, 0x69, 0x64, 0x65
                });
                algorithm.Padding = PaddingMode.ISO10126;
                algorithm.Key = rdb.GetBytes(32);
                algorithm.IV = rdb.GetBytes(16);
                return algorithm;
            }
        }


        /// <summary>
        ///     Encrypts a string with a given password.
        /// </summary>
        /// <param name="clearBytes">The clear Bytes.</param>
        /// <param name="password">The password.</param>
        public static byte[] EncryptBytes(byte[] clearBytes, string password)
        {
            SymmetricAlgorithm algorithm = GetAlgorithm(password);
            ICryptoTransform encryptor = algorithm.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Decrypts a string using a given password.
        /// </summary>
        /// <param name="cipherBytes">The cipher Bytes.</param>
        /// <param name="password">The password.</param>
        public static byte[] DecryptBytes(byte[] cipherBytes, string password)
        {
            SymmetricAlgorithm algorithm = GetAlgorithm(password);
            ICryptoTransform decryptor = algorithm.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.Close();
                return ms.ToArray();
            }
        }
    }
}
