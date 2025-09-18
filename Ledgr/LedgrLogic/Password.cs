namespace LedgrLogic;
using System;
using System.Security.Cryptography;
using System.Text;

public class Password
{
    
    public static string Validate(string password)
    {
        bool hasNum = false;
        bool hasSpec = false;

        if (password.Length < 8)
        {
            if (Char.IsLetter(password[0]))
            {
                for (int i = 0; i < password.Length; i++)
                {
                    if (Char.IsDigit(password[i])) {hasNum = true;}
                    if (!Char.IsLetterOrDigit(password[i])) {hasSpec = true;}
                }
            }
            else { return "First character must be a letter"; }
        }
        else { return "Password must be at least 8 characters long"; }
        if (!hasNum) { return "Password must contain a number"; }
        if (!hasSpec) { return "Password must contain a special character"; }
        return "Success";
    }
    
    
    public static string Encrypt(string password)
    {
        // This value can be changed, but it must be 16 characters (or 24, 32)
        // However, the same key MUST be used to decrypt the passwords. Will be discussed.
        string key = "LedgrPrivateEkey";
        
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            // Needs to match characters in key as well as size of byte array in decryption algorithm
            aes.IV = new byte[16];
            
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write);
            
            // Trying to determine why StreamWriter requires "using"
            using (StreamWriter sWriter = new StreamWriter(cStream))
            {
                sWriter.Write(password);
            }
            return Convert.ToBase64String(mStream.ToArray());
        }
    }
    
    public static string Decrypt(string encryptedPassword)
    {
        string key = "LedgrPrivateEkey";
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16]; 

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            MemoryStream mStream = new MemoryStream(Convert.FromBase64String(encryptedPassword));
            CryptoStream cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);
            StreamReader sReader = new StreamReader(cStream);
            return sReader.ReadToEnd();
        }
    }
}