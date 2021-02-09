using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Security.Cryptography;

/// <summary>
///AES 的摘要说明
/// </summary>
/// <summary>
/// AES加密解密类
/// </summary>
public class AES
{
    /// <summary>
    ///    Decrypts  a particular string with a specific Key
    ///    用一个特殊的钥匙解密加密的字符串,如果解密失败，返回
    ///    <param name="stringToDecrypt">要解密的字符串</param>
    ///    <param name="sEncryptionKey">解密所用的钥匙</param>
    /// </summary>
    public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
    {
        if (stringToDecrypt == null)
        {
            return string.Empty;
        }

        sEncryptionKey = sEncryptionKey.PadLeft(16, '0');

        byte[] key = { };
        byte[] inputByteArray = Convert.FromBase64String(stringToDecrypt);
        try
        {
            key = Encoding.Unicode.GetBytes(sEncryptionKey.Substring(0, 16));

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = key;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);

            return Encoding.Unicode.GetString(resultArray);
        }
        catch
        {
            return (string.Empty);
        }
    }

    /// <summary>
    ///   Encrypts  a particular string with a specific Key
    ///   根据钥匙加密字符串
    /// </summary>
    /// <param name="stringToEncrypt">要加密的字符串</param>
    /// <param name="sEncryptionKey">加密的钥匙</param>
    /// <returns></returns>
    public static string Encrypt(string stringToEncrypt, string sEncryptionKey)
    {
        if (stringToEncrypt == null)
        {
            return "";
        }
        sEncryptionKey = sEncryptionKey.PadLeft(16, '0');
        byte[] key = { };
        byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length) 

        try
        {
            key = Encoding.Unicode.GetBytes(sEncryptionKey.Substring(0, 16));
            inputByteArray = Encoding.Unicode.GetBytes(stringToEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = key;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }
        catch
        {
            return (string.Empty);
        }
    }

    /// <summary>
    /// 解密,按默认密钥
    /// </summary>
    /// <param name="strRec"></param>
    /// <returns></returns>
    public static string Decrypt(string strRec)
    {
        return Decrypt(strRec, ok());
    }

    private static string ok()
    {
        StringBuilder sb = new StringBuilder(10);
        byte[] bt = { 50, 48, 48, 57, 48, 57, 48, 49 };//20090901
        for (int i = 0; i < bt.Length; i++)
        {
            sb.Append((char)bt[i]);
        }
        return sb.ToString();
    }
    /// <summary>
    /// 加密,按默认密钥
    /// </summary>
    /// <param name="strRec"></param>
    /// <returns></returns>
    public static string Encrypt(string strRec)
    {
        return Encrypt(strRec, ok());
    }
}
