using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DotnetLibraries
{
    /// <summary>
    /// 加解密
    /// </summary>
    /// <remarks>
    /// AES (Advanced Encryption Standard): 對稱式加密演算法，被廣泛用於許多安全應用程式中。它是一種快速、安全且效率高的加密演算法，支援128位、192位和256位金鑰。
    ///     - 同一把鑰匙, 速度快, 長度沒限制
    ///     - 加密模式預設為 CBC
    ///     - 金鑰預設長度為 256
    /// RSA (Rivest-Shamir-Adleman): 非對稱式加密演算法，廣泛用於數字簽名和金鑰交換。它基於大數的分解難題，使用公開金鑰和私有金鑰來加密和解密資料。
    ///     - 兩把鑰匙, 加密只需要公鑰, 解密才需要私鑰, 速度慢, 長度受限
    ///     - 金鑰預設長度為 1024
    ///     - 金鑰長度為 2048, 才能支援 245 個字元
    /// SHA (Secure Hash Algorithm): 摘要演算法, 被廣泛用於許多安全應用程式中，如數字簽名、訊息驗證、密碼存儲等。
    ///     - 是一系列的雜湊函數，用於將任意長度的輸入訊息轉換為固定長度的摘要。
    ///     - 單向的，即從摘要很難恢復出原始訊息。
    ///     - MD5 (不建議使用, 32 個字元): 可以產生出一個 128 位元 (16 個位元組) 的雜湊值 (hash value), 用於確保資訊傳輸完整一致。
    ///     - SHA-256、SHA-384、SHA-512 (SHA-2 系列): SHA-2系列是一系列安全的雜湊函數，廣泛用於許多安全應用程式中。它們提供了更高的安全性和較大的摘要大小，分別支援256位、384位和512位的摘要。
    ///         - SHA-256: 64 個字元
    ///         - SHA-512: 128 個字元
    /// HMAC (Hash-based Message Authentication Code): HMAC是一種訊息驗證碼，基於安全的雜湊函數（如SHA系列），使用秘密金鑰對訊息進行加密以驗證其完整性和真實性。
    ///     - 相同之處：
    ///         - 基於雜湊函數：SHA 和 HMAC 都是基於雜湊函數的加密算法，用於產生固定長度的摘要。
    ///         - 用於訊息驗證：SHA 和 HMAC 都可以用於訊息驗證，以確保訊息的完整性和真實性。
    ///     - 不同之處：
    ///         - 用途：
    ///             - SHA：主要用於產生訊息的摘要，以便識別和比較兩個訊息是否相同。
    ///             - HMAC：主要用於產生帶有密鑰的訊息驗證碼，以防止訊息被竄改或篡改。
    ///         - 使用方式：
    ///             - SHA：單獨使用 SHA 通常是公開的，而且不需要密鑰。SHA 摘要是基於訊息本身的。
    ///             - HMAC：HMAC 需要一個密鑰，這個密鑰用於計算 HMAC，並且只有知道這個密鑰的人才能夠驗證 HMAC 的真實性。
    ///         - 安全性：
    ///             - HMAC：由於 HMAC 使用了密鑰，因此它比純粹的 SHA 更安全，能夠防止更多的攻擊，如密碼學中的篡改攻擊。
    ///             - SHA：雖然 SHA 也是安全的，但它的安全性取決於算法的選擇和實現。
    ///         - 應用場景：
    ///             - SHA：用於數字簽名、訊息摘要、數據完整性驗證等。
    ///             - HMAC：用於訊息驗證、訪問令牌生成、API 認證、身份驗證、數據完整性檢查等需要身份驗證和安全性保護的場景。
    /// 
    /// reference: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography?view=net-8.0
    /// </remarks>
    public static class CryptographyHelper
    {
        #region RSA by XmlString

        public static RSAParameters GenerateRSAKeys(string xml)
        {
            // 判斷是否有 XML 標籤, 總共 6 組
            var regex = new Regex(@"<P>\S+</P>|<Q>\S+</Q>|<DP>\S+</DP>|<DQ>\S+</DQ>|<InverseQ>\S+</InverseQ>|<D>\S+</D>",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matched = regex.Matches(xml);
            bool includePrivateParameters = matched.Count == 6;

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xml);
                return rsa.ExportParameters(includePrivateParameters);
            }
        }

        // https://dotblogs.com.tw/supershowwei/2015/12/23/160510
        public static Tuple<string, string> GenerateRSAKeys(int dwKeySize = 1024)
        {
            using (var rsa = new RSACryptoServiceProvider(dwKeySize))
            {
                var publicKey = rsa.ToXmlString(false);
                var privateKey = rsa.ToXmlString(true);

                return Tuple.Create<string, string>(publicKey, privateKey);
            }
        }

        public static string EncryptRSA(string publicKey, string text)
        {
            byte[] encryptedData;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] data = Encoding.UTF8.GetBytes(text);
                encryptedData = rsa.Encrypt(data, false);
            }

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptRSA(string privateKey, string encryptedContent)
        {
            byte[] decryptedData;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);

                byte[] data = Convert.FromBase64String(encryptedContent);
                decryptedData = rsa.Decrypt(data, false);
            }

            return Encoding.UTF8.GetString(decryptedData);
        }

        #endregion

        #region RSA by Parameters

        public static string GenerateRSAParameters(RSAParameters info)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(info);
                bool isNullOrEmpty =
                    IsNullOrEmpty(info.P) & IsNullOrEmpty(info.Q) &
                    IsNullOrEmpty(info.DP) & IsNullOrEmpty(info.DQ) &
                    IsNullOrEmpty(info.InverseQ) & IsNullOrEmpty(info.D);
                bool includePrivateParameters = isNullOrEmpty == false;

                return rsa.ToXmlString(includePrivateParameters);
            }
        }

        /// <summary>Indicates whether the specified array is null or has a length of zero.</summary>
        /// <param name="array">The array to test.</param>
        /// <returns>true if the array parameter is null or has a length of zero; otherwise, false.</returns>
        public static bool IsNullOrEmpty(Array array)
        {
            return (array == null || array.Length == 0);
        }

        public static RSAParameters GenerateRSAParameters(bool includePrivateParameters)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return rsa.ExportParameters(includePrivateParameters);
            }
        }

        public static string EncryptRSA(RSAParameters info, string text)
        {
            byte[] encryptedData;
            // Create a new instance of RSACryptoServiceProvider.
            using (var rsa = new RSACryptoServiceProvider())
            {

                // Import the RSA Key information. This only needs
                // toinclude the public key information.
                rsa.ImportParameters(info);

                // Encrypt the passed byte array and specify OAEP padding.  
                // OAEP padding is only available on Microsoft Windows XP or
                // later.  
                byte[] data = Encoding.UTF8.GetBytes(text);
                encryptedData = rsa.Encrypt(data, false);
            }

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptRSA(RSAParameters info, string text)
        {
            byte[] decryptedData;
            // Create a new instance of RSACryptoServiceProvider.
            using (var rsa = new RSACryptoServiceProvider())
            {
                // Import the RSA Key information. This needs
                // to include the private key information.
                rsa.ImportParameters(info);

                // Decrypt the passed byte array and specify OAEP padding.  
                // OAEP padding is only available on Microsoft Windows XP or
                // later.  
                byte[] data = Convert.FromBase64String(text);
                decryptedData = rsa.Decrypt(data, false);
            }

            return Encoding.UTF8.GetString(decryptedData);
        }

        #endregion

        #region AES from MSDN

        // reference: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0

        public static Tuple<byte[], byte[]> GenerateAES()
        {
            using (Aes myAes = Aes.Create())
            {
                return Tuple.Create(myAes.Key, myAes.IV);
            }
        }

        public static string EncryptAES(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted = EncryptStringToBytes_Aes(plainText, Key, IV);
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptAES(string cipherText, byte[] Key, byte[] IV)
        {
            byte[] encrypted = Convert.FromBase64String(cipherText);
            return DecryptStringFromBytes_Aes(encrypted, Key, IV);
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {

                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }

        #endregion

        #region AES

        [Obsolete("The Rijndael and RijndaelManaged types are obsolete. Use Aes instead.", false)]
        private static RijndaelManaged GetAES(string key, string iv)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(key);
            byte[] bIV = Encoding.UTF8.GetBytes(iv);
            // 32 * 8 = 256
            if (bKey.Length != 32)
            {
                throw new Exception("AES key bit length in bytes should be 256.");
            }
            // 16 * 8 = 128
            if (bIV.Length != 16)
            {
                throw new Exception("AES IV bit length in bytes should be 128.");
            }
            var aes = new RijndaelManaged();
            aes.Key = bKey;
            aes.IV = bIV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        /// <summary>
        ///  Encrypt with AES256
        /// </summary>
        /// <param name="source">本文</param>
        /// <param name="key">256 bits as key, i.g.: "12345678901234567890123456789012"</param>
        /// <param name="iv">128 bits as initialization vector, i.g.: "1234567890abcdef"</param>
        /// <returns></returns>
        public static string EncryptAES256(string source, string key, string iv)
        {
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            var aes = GetAES(key, iv);
            ICryptoTransform transform = aes.CreateEncryptor();
            return Convert.ToBase64String(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));
        }

        /// <summary>
        /// Decrypt with AES256
        /// </summary>
        /// <param name="encryptData">>Encrypt string in base64</param>
        /// <param name="key">256 bits as key, i.g.: "1234567890abcdef"</param>
        /// <param name="iv">128 bits as initialization vector</param>
        /// <returns></returns>
        public static string DecryptAES256(string encryptData, string key, string iv)
        {
            var encryptBytes = Convert.FromBase64String(encryptData);
            var aes = GetAES(key, iv);
            ICryptoTransform transform = aes.CreateDecryptor();
            return Encoding.UTF8.GetString(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length));
        }

        #endregion

        #region MD5

        [Obsolete("Consider using the SHA256 class or the SHA512 class instead of the MD5 class", false)]
        public static string EncryptMD5(string text)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        #endregion

        #region SHA

        public static string EncryptSHA256(string text)
        {
            using (SHA256 shaM = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = shaM.ComputeHash(inputBytes);
                return GetStringFromHash(hashBytes);
            }
        }

        public static string EncryptSHA512(string text)
        {
            using (SHA512 shaM = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = shaM.ComputeHash(inputBytes);
                return GetStringFromHash(hashBytes);
            }
        }

        // Convert the byte array to hexadecimal string
        // reference: https://gist.github.com/obrassard/766951b3c65984273ce4b6475e568cf5
        private static string GetStringFromHash(byte[] hashBytes)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                result.Append(hashBytes[i].ToString("X2"));
            }
            return result.ToString();
        }

        #endregion
    }
}
