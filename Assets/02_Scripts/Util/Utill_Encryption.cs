using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Security.Cryptography;
using System.Text;


/// <summary>
///  OVE Encryption_Guide.pdf Âü°í.
///    ¤¤ .net 3.5 higher
/// </summary>

public partial class Utill
{
    static byte[] Encrypt(byte[] data, byte[] key)
    {
        using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
        {

            csp.KeySize = 256;

            csp.BlockSize = 128;

            csp.Key = key;

            csp.Padding = PaddingMode.PKCS7;

            csp.Mode = CipherMode.ECB;

            ICryptoTransform encrypter = csp.CreateEncryptor();

            return encrypter.TransformFinalBlock(data, 0, data.Length);
        }
    }
    static byte[] Decrypt(byte[] data, byte[] key)
    {
        using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
        {

            csp.KeySize = 256;

            csp.BlockSize = 128;

            csp.Key = key;

            csp.Padding = PaddingMode.PKCS7;

            csp.Mode = CipherMode.ECB;

            ICryptoTransform decrypter = csp.CreateDecryptor();

            return decrypter.TransformFinalBlock(data, 0, data.Length);
        }

    }

    public static byte[] Encrypt_byte(string data, string key = SALT)
    {
        byte[] bdata = Encoding.UTF8.GetBytes(data);
        byte[] bkey = Encoding.Default.GetBytes(key);

        byte[] bresult = Encrypt(bdata, bkey);

        return bresult;
    }
    public static string Encrypt_string(string data, string key = SALT)
    {
        byte[] bdata = Encoding.UTF8.GetBytes(data);
        byte[] bkey = Encoding.Default.GetBytes(key);

        byte[] bresult = Encrypt(bdata, bkey);

        return System.Convert.ToBase64String(bresult);
    }
    public static string Decrypt(string data, string key = SALT)
    {
        byte[] bdata = System.Convert.FromBase64String(data);// Encoding.UTF8.GetBytes(data);
        byte[] bkey = Encoding.Default.GetBytes(key);

        byte[] bresult = Decrypt(bdata, bkey);

        return Encoding.UTF8.GetString(bresult);
    }
    public static string Decrypt_FromByte(byte[] data, string key = SALT)
    {
        //byte[] bdata = System.Convert.FromBase64String(data);// Encoding.UTF8.GetBytes(data);

        byte[] bkey = Encoding.Default.GetBytes(key);
        byte[] bresult = Decrypt(data, bkey);

        return Encoding.UTF8.GetString(bresult);
    }
    public const string SALT = "The Best Idol is BlackPink Jisoo";
    // 32byte
    private static string getString(byte[] b)
    {
        return Encoding.UTF8.GetString(b);
    }

    public static void Test()
    {
        byte[] data = Encoding.UTF8.GetBytes("AES-256-ECB Encoding Test!");
        Encoding byteEncoder = Encoding.Default;

        byte[] key = byteEncoder.GetBytes(SALT);
        byte[] enc = Encrypt(data, key);

        string result;
        result = System.Convert.ToBase64String(enc);
        byte[] dec = Decrypt(enc, key);

        Debug.Log(result.Length);
        Debug.Log(enc.Length);

        Debug.LogFormat("Input : {0}", getString(data));
        Debug.LogFormat("Key : {0}", getString(key));
        Debug.LogFormat("KeySize : {0}", 256);
        Debug.LogFormat("Encrypted : {0}", result);
        Debug.LogFormat("Decrypted : {0}", getString(dec));
    }
}
