using System.Globalization;
using Microsoft.JSInterop;
using MoneyManagement_Web.Interfaces;

namespace MoneyManagement_Web.Services
{
    public class UtilityServices : IUtilityServices
    {
        
        private readonly IConfiguration _config;
        private readonly IJSRuntime _runtime;

        private readonly SessionStorageAccessor SessionStorageAccessor;

        public UtilityServices(IConfiguration config, IJSRuntime jSRuntime, SessionStorageAccessor sessionStorageAccessor)
        {
            _config = config;
            _runtime = jSRuntime;
            SessionStorageAccessor = sessionStorageAccessor;
        }

        public string FormatAsEUR(object value)
        {
            if (value == null)
            {
                return "00";
            }

            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("it-IT"));
        }

        public string FormatAsDate(object value)
        {
            if (value != null)
            {
                return Convert.ToDateTime(value).ToString("MMM yyyy");
            }

            return "--";
        }

        public string FormatAsCurrency(double amountValue, string currency)
        {
            switch (currency)
            {
                case "EUR":
                    return ((double)amountValue).ToString("C0", CultureInfo.CreateSpecificCulture("it-IT"));

                case "CHF":
                    return ((double)amountValue).ToString("C0", CultureInfo.CreateSpecificCulture("ch-CH"));


                default:
                    return ((double)amountValue).ToString("C0", CultureInfo.CreateSpecificCulture("us-US"));

            }


        }

        public string GetRestUrl()
        {
            var uri = _config.GetSection("Uri").Value;
            return uri;
        }

        public async Task CopyToClipboard(string text)
        {
            await _runtime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }


        public async Task WriteLog(string value)
        {
            var _text = string.Concat(DateTime.Now.ToString(), " - ", value, Environment.NewLine);
            var log = string.Concat(await SessionStorageAccessor.GetValueAsync<string>("LOG"), _text);

            await SessionStorageAccessor.SetValueAsync("LOG", log);
        }

        public async Task<string> GetLog()
        {
            var StoredValue = await SessionStorageAccessor.GetValueAsync<string>("LOG");
            return StoredValue;
            
        }

        public async Task ClearLog()
        {
            await SessionStorageAccessor.RemoveAsync("LOG");

        }

        public string FileSizeFormatted(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }
    }
}
