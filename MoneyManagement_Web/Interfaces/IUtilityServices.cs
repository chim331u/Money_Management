namespace MoneyManagement_Web.Interfaces
{
    public interface IUtilityServices
    {
        string GetRestUrl();
        Task CopyToClipboard(string text);
        string FormatAsEUR(object value);
        string FormatAsDate(object value);
        string FormatAsCurrency(double amountValue, string currency);

        Task WriteLog(string value);
        Task<string> GetLog();
        Task ClearLog();
        string FileSizeFormatted(double len);
    }
}
