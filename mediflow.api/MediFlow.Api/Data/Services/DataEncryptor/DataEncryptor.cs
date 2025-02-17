using MediFlow.Api.Data.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MediFlow.Api.Data.Services.DataEncryptor;

public sealed class DataEncryptor : IDataEncryptor
{
    public DataEncryptor(IOptions<DataSecurityOptions> securityOptions)
    {
        if (securityOptions.Value is null) throw new ArgumentNullException(nameof(securityOptions.Value));
        keyBytes = Encoding.UTF8.GetBytes(securityOptions.Value.EncryptionKeys.Current);
    }

    private readonly byte[] keyBytes;


    public T Decrypt<T>(string ciphertext)
    {
        if (ciphertext.Length < 16) throw new Exception("encrypted data must be corrupted");
        string decryptedData = string.Empty;

        ciphertext = new StringBuilder(ciphertext)
                .Replace("%2F", "/")
                .Replace("%3D", "=")
                .ToString();
        byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

        byte[] ivBytes = new byte[16];
        Array.Copy(ciphertextBytes, ivBytes, ivBytes.Length);

        var dataBytes = new byte[ciphertextBytes.Length - ivBytes.Length];
        Array.Copy(ciphertextBytes, ivBytes.Length, dataBytes, 0, dataBytes.Length);

        using (Aes aes = Aes.Create())
        {
            ICryptoTransform decryptor = aes.CreateDecryptor(keyBytes, ivBytes);
            using (MemoryStream memoryStream = new MemoryStream(dataBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        decryptedData = streamReader.ReadToEnd();
                    }
                }
            }
        }

        return Deserialize<T>(decryptedData);
    }

    public string Encrypt<T>(T data)
    {
        byte[] iv = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(iv);

        var serialized = Serialize(data);
        byte[] cipheredtext;

        using (Aes aes = Aes.Create())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(keyBytes, iv);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(serialized);
                    }

                    cipheredtext = memoryStream.ToArray();
                }
            }
        }

        var resultArr = new byte[iv.Length + cipheredtext.Length];
        iv.CopyTo(resultArr, 0);
        cipheredtext.CopyTo(resultArr, iv.Length);
        var encrypted = Convert.ToBase64String(resultArr);
        return new StringBuilder(encrypted)
                .Replace("/", "%2F")
                .Replace("=", "%3D")
                .ToString();
    }

    private string Serialize(object? data) => JsonSerializer.Serialize(data);
    private T Deserialize<T>(string serialized) => JsonSerializer.Deserialize<T>(serialized) ?? throw new Exception("couldnt deserialize data");

}
