using UnityEngine;
using System.IO;

public class CredentialsManager : MonoBehaviour
{
    // No need to reference CryptoManager as an object since the methods are static

    public string LoadCredentials()
    {
        string path = Application.dataPath + "/credentials.txt";
        if (File.Exists(path))
        {
            string encryptedCredentials = File.ReadAllText(path);
            return CryptoManager.Decrypt(encryptedCredentials);  // Call Decrypt on the class, not an instance
        }
        else
        {
            Debug.LogError("Credentials file not found!");
            return null;
        }
    }

    public void SaveCredentials(string username, string password)
    {
        string credentials = username + ":" + password;
        string encryptedCredentials = CryptoManager.Encrypt(credentials);  // Call Encrypt on the class, not an instance
        string path = Application.dataPath + "/credentials.txt";
        File.WriteAllText(path, encryptedCredentials);
    }
}
