using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordSafe
{
    internal class Crypting
    {
        private static readonly byte[] saltBytes = { 90, 239, 224, 27, 215, 52, 244, 198, 195, 120, 49, 131, 119, 43 };
        public static string EncryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);

            byte[] baSalt = GetRandomBytes();
            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

            // Combine Salt + Text
            for (int i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (int i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public static string DecryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);
            byte[] baText = Convert.FromBase64String(text);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256.Create().ComputeHash(baPwd);

            byte[] baDecrypted = AES_Decrypt(baText, baPwdHash);

            // Remove salt
            int saltLength = SaltLength;
            byte[] baResult = new byte[baDecrypted.Length - saltLength];
            for (int i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + saltLength];

            string result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private static byte[] GetRandomBytes()
        {
            byte[] ba = new byte[SaltLength];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }

        private static int SaltLength { get; set; } = 10;
    }
}
