using System;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace AVMCoE.Framework
{
    /// <summary>
    /// This class holds  AESEncryption
    /// </summary>
    public class AESEncryption
    {
        /// <summary>
        /// Method to encrypt string
        /// </summary>
        /// <param name="plainText">This parameter holds plainText value</param>
        /// <param name="Key"> This parameter holds Key value</param>
        /// <returns></returns>
        public byte[] EncryptStringAsBytes(string plainText, byte[] Key)
        {

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;

            Aes aesAlg = null;
            byte[] IV = new byte[16];

            try
            {
                aesAlg = Aes.Create();
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Key;
                aesAlg.GenerateIV();
                aesAlg.IV.CopyTo(IV, 0);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);
                swEncrypt.Write(plainText);
            }
            finally
            {

                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            var encArray = msEncrypt.ToArray();
            byte[] encStream = new byte[encArray.Length + 16];
            Array.Copy(encArray, encStream, encArray.Length);
            Array.Copy(IV, 0, encStream, encArray.Length, 16);
            return encStream;
        }

        private byte[] returnIV(byte[] iV)
        {
            return iV;
        }

        /// <summary>
        /// Method to Decrypt the cipherText
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string DecryptStringFromBytes(byte[] cipherText, byte[] Key)
        {
            string decodedText = null;
            if (cipherText != null || cipherText.Length >= 32)
            {
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                MemoryStream msDecrypt = null;
                CryptoStream csDecrypt = null;
                StreamReader srDecrypt = null;
                Aes aesAlg = null;

                try
                {
                    aesAlg = Aes.Create();
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Key = Key;
                    byte[] IV = new byte[16];
                    Array.Copy(cipherText, cipherText.Length - 16, IV, 0, 16);
                    aesAlg.IV = returnIV(IV);
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    msDecrypt = new MemoryStream(cipherText, 0, cipherText.Length - 16);
                    csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    srDecrypt = new StreamReader(csDecrypt);
                    decodedText = HttpUtility.HtmlEncode(srDecrypt.ReadToEnd());
                }
                catch
                {
                    decodedText = string.Empty;
                    msDecrypt = null;
                    csDecrypt = null;
                    srDecrypt = null;

                }
                finally
                {
                    if (srDecrypt != null)
                        srDecrypt.Close();
                    if (csDecrypt != null)
                        csDecrypt.Close();
                    if (msDecrypt != null)
                        msDecrypt.Close();
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
                return HttpUtility.HtmlDecode(decodedText);
            }

            return HttpUtility.HtmlDecode(decodedText);
        }

        /// <summary>
        /// Method to Decrypt the TicketDescription
        /// </summary>
        /// <param name="description">description</param>
        /// <param name="Key">Key</param>
        /// <returns>Decrypted String</returns>
        public string DecryptStringBytes(string description, byte[] Key)
        {
            string decodedText = null;
            try

            {
                // Create an Aes object
                decodedText = DecryptStringFromBytes(Convert.
                           FromBase64String((string)description), Key);
            }
            // Read the errored string to avoid load issues 
            // and place them in a string..
            catch
            {
                decodedText = string.Empty;
            }
            return HttpUtility.HtmlDecode(decodedText);

        }

    }
}
