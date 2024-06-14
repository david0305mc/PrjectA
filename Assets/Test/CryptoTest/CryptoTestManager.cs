using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;
using Cysharp.Threading.Tasks;
using System.Text;

public class CryptoTestManager : MonoBehaviour
{
    private string[] KEY =  {"ITzKFvm5VRwj2oyJAytAHbzEqru9oAHY",
        "770A8A65DA156D24EE2A093277530142",
        "BGQSDb5fjKQMx2GlK2pgUkBp0oGA1XBP",
        "aFTwKX013M9A2y9FOYLtQm6VtYa4ECuT",
        "WVHzA2oHVjtidipsRWi2P54sW7FRqOv1",
        "AZprwFetViIxq28dYDiga4DC1QQgybvk",
        "4aJTqnQmCOog31SxZCzV8QUpGxI5vgJI",
        "dnYfeVWgjauxR/b+uXFsZ9FoDefX2p7T"};

    //private readonly string key = "ITzKFvm5VRwj2oyJAytAHbzEqru9oAHY";
    private readonly byte [] IV = new byte[] { 15, 57, 69, 79, 204, 145, 125, 100, 95, 134, 188, 50, 49, 171, 251, 234 };
    

    private void Start()
    {
        WebRequestTest().Forget();
    }
    private async UniTask WebRequestTest()
    {   
        string fileName = "Abyss_ad.csv";
        Dictionary<string, string> Headers = new Dictionary<string, string>() { { "Cache-Control", "no-cache, no-store" } };
        string url = string.Format("https://d2ai02pqpi5le7.cloudfront.net/gamedata/dev/{0}", fileName);
        byte[] data = await UnityHttp.Get(url, Headers);
        
        //await File.WriteAllBytesAsync(string.Format($"{PathInfo.DataPath}/{ServerSettings.serverName}/CryptTest.csv"), data);

        using (Rijndael myRijndael = Rijndael.Create())
        {
                
            //var rijndaelKey = Encoding.UTF8.GetBytes(key);

            string path = $"{Application.dataPath}/Test/CryptoTest/Test1114.csv";
            // Encrypt the string to an array of bytes.
            System.Random r = new System.Random();
            int keyValue = r.Next(0, 7);

            RijndaelExample.EncryptBytesToFile(data, path, KEY[keyValue], IV);
            // Decrypt the bytes to a string.
            string roundtrip = RijndaelExample.DecryptStringFromFile(path, KEY[keyValue], IV);

            ////Display the original data and the decrypted data.
            //Debug.Log(string.Format("Original:   {0}", original));
            Debug.Log(string.Format("Round Trip: {0}", roundtrip));
        }
    }
    private void CryptTestFromMSDN()
    {
        try
        {
            string original = "Here is some data to encrypt!";

            // Create a new instance of the Rijndael
            // class.  This generates a new key and initialization
            // vector (IV).
            using (Rijndael myRijndael = Rijndael.Create())
            {
                // Encrypt the string to an array of bytes.
                byte[] encrypted = RijndaelExample.EncryptStringToBytes(original, myRijndael.Key, myRijndael.IV);

                // Decrypt the bytes to a string.
                string roundtrip = RijndaelExample.DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);

                //Display the original data and the decrypted data.
                Debug.Log(string.Format("Original:   {0}", original));
                Debug.Log(string.Format("Round Trip: {0}", roundtrip));
            }
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("Error: {0}", e.Message));
        }
    }
}

public class RijndaelExample
{
    public static async UniTask EncryptBytesToFileAsync(byte[] bytesToEncrypt, string path, string key, byte[] IV)
    {
        // Check arguments.
        if (bytesToEncrypt == null || bytesToEncrypt.Length <= 0)
            throw new ArgumentNullException("bytesToEncrypt");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        // Create an Rijndael object
        // with the specified key and IV.
        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = Encoding.UTF8.GetBytes(key);
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                using (CryptoStream csEncrypt = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                {
                    await csEncrypt.WriteAsync(bytesToEncrypt, 0, bytesToEncrypt.Length);
                }
            }
        }
    }
    public static void EncryptBytesToFile(byte[] bytesToEncrypt, string path, string key, byte[] IV)
    {
        // Check arguments.
        if (bytesToEncrypt == null || bytesToEncrypt.Length <= 0)
            throw new ArgumentNullException("bytesToEncrypt");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        // Create an Rijndael object
        // with the specified key and IV.
        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = Encoding.UTF8.GetBytes(key);
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                using (CryptoStream csEncrypt = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                }
            }
        }
    }

    public static void EncryptStringToFile(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        // Create an Rijndael object
        // with the specified key and IV.
        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using (var fileStream = new FileStream($"{Application.dataPath}/Test/CryptoTest/CryptoTestEncrypted2.csv", FileMode.Create))
            {
                using (CryptoStream csEncrypt = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }
            }
        }
    }
    public static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;
        // Create an Rijndael object
        // with the specified key and IV.
        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

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
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
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

        // Create an Rijndael object
        // with the specified key and IV.
        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

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
        }

        return plaintext;
    }
    public static string DecryptStringFromFile(string path, string key, byte[] IV)
    {
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an Rijndael object
        // with the specified key and IV.
        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = Encoding.UTF8.GetBytes(key);
            rijAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            using (var msDecrypt = new FileStream(path, FileMode.Open))
            {

                //byte[] temp = new byte[5];
                //msDecrypt.Read(temp, 0, 5);
                //Debug.Log(temp);
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
        }

        return plaintext;
    }
}
