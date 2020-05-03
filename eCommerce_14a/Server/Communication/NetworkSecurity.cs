using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Communication
{
    public class NetworkSecurity
    {
        private Aes aesAlg;
        ICryptoTransform encryptor;
        ICryptoTransform decryptor;

        public NetworkSecurity()
        {
            aesAlg = Aes.Create();
            aesAlg.Key = GetKey();
            aesAlg.IV = GetInitialVector();
            encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        }

        public byte[] Encrypt(string plainText) 
        {
            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
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

        public string Decrypt(byte[] cipherText) 
        {
            string plaintext = null;
                // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
          return plaintext;
        }

        public byte[] GetKey() 
        {
            byte seed = 1;
            byte[] key = new byte[aesAlg.KeySize / 8];
            for (int i = 0; i < key.Length; i++) 
            {
                key[i] = seed;
                seed += 1;
            }
            return key;
        }

        public byte[] GetInitialVector() 
        {
            byte seed = 255;
            byte[] IV = new byte[aesAlg.BlockSize / 8];
            for (int i = 0; i < IV.Length; i++)
            {
                IV[i] = seed;
                seed -= 1;
            }
            return IV;
        }
    }
}

