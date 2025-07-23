using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using MoneyManagement_Api.AppContext;
using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.IdentityAccess;

namespace MoneyManagement_Api.Services;

public class UtilityService : IUtilityService
{
    private readonly ApplicationContext _context;
    private string myKey;
    private readonly ILogger<UtilityService> _logger;

    public UtilityService(ILogger<UtilityService> logger, ApplicationContext context)
    {
        _context = context;
        _logger = logger;
        myKey = _context.ServiceConfigs.Where(k => k.Key == "CryptoKey").FirstOrDefault().Value.ToString();
    }

    //public string WriteLog(LogType logType, string message)
    //{
    //    var logMessage = $"{DateTime.Now}  [{logType}] - {message}";

    //    Console.WriteLine(logMessage);

    //    return logMessage + Environment.NewLine;
    //}

    #region DataProtection

    public string EncryptString(string plainText)
    {
        var iv = new byte[16];
        byte[] array;

        var key = "";

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    public string EncryptString(ISA_Accounts item)
    {
        var key = string.Concat(myKey, item.CreatedDate.ToString(CultureInfo.InvariantCulture));
        var plainText = item.Password;
        var iv = new byte[16];
        byte[] array;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    public string DecryptString(string cipherText)
    {
        var iv = new byte[16];
        var buffer = Convert.FromBase64String(cipherText);
        var key = "";

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var memoryStream = new MemoryStream(buffer))
            {
                using (var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

    public string DecryptString(ISA_Accounts item)
    {
        try
        {
            var key = string.Concat(myKey, item.CreatedDate.ToString(CultureInfo.InvariantCulture));
            var cipherText = item.Password;
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(cipherText);


            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            var clearPsw = streamReader.ReadToEnd();
                            return clearPsw;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    #endregion


    public string TimeDiff(DateTime start, DateTime end)
    {
        var _span = end - start;
        return string.Concat("[", ((int)_span.TotalMilliseconds).ToString(), " ms]");
    }
}