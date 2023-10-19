using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class CryptoManager : MonoBehaviour
{
    // It's better to use a more secure method to store and access these values.
    private static readonly string Key = "";  // 16 chars = 128 bits
    private static readonly string IV = "";  // 16 chars = 128 bits
    private static readonly string EncryptedUsername = "encrypted-username-here";
    private static readonly string EncryptedPassword = "e58b69ad62f34002a344af2e6a7a19bd";

    public static string DecryptUsername()
    {
        return Decrypt(EncryptedUsername);
    }

    public static string DecryptPassword()
    {
        return Decrypt(EncryptedPassword);
    }

    public static string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Key);
            aesAlg.IV = Encoding.UTF8.GetBytes(IV);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Key);
            aesAlg.IV = Encoding.UTF8.GetBytes(IV);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
