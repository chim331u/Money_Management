using MoneyManagement_Web.Data.AncillaryData;

namespace MoneyManagement_Web.Interfaces
{
    public interface IAnchillaryService
    {
        Task<List<Country>> GetActiveCountryList();
        Task<Country> GetCountry(int countryId);
        Task<Country> AddCountry(Country country);
        Task<Country> UpdateCountry(Country country);
        Task<Country> DeleteCountry(Country country);

        Task<List<Currency>> GetActiveCurrencyList();
        Task<Currency> GetCurrency(int currencyId);
        Task<Currency> AddCurrency(Currency currency);
        Task<Currency> UpdateCurrency(Currency currency);
        Task<Currency> DeleteCurrency(Currency currency);

        Task<List<CurrencyConversionRate>> GetActiveCurrencyConversionList();
        Task<CurrencyConversionRate> GetCurrencyConversion(int currencyConversionRateId);
        Task<CurrencyConversionRate> GetCurrencyRate(string currencyALF3);
        Task<string> UpdateCurrencyRates();
        Task<string> ClearUnusedRates();
        Task<CurrencyConversionRate> AddCurrencyConversion(CurrencyConversionRate currencyConversionRate);
        Task<CurrencyConversionRate> UpdateCurrencyConversion(CurrencyConversionRate currencyConversionRate);
        Task<CurrencyConversionRate> DeleteCurrencyConversion(CurrencyConversionRate currencyConversionRate);

        Task<List<ServiceUser>> GetActiveServiceUserList();
        Task<ServiceUser> GetServiceUser(int serviceUserId);
        Task<ServiceUser> AddServiceUser(ServiceUser serviceUser);
        Task<ServiceUser> UpdateServiceUser(ServiceUser serviceUser);
        Task<ServiceUser> DeleteServiceUser(ServiceUser serviceUser);

        Task<List<Supplier>> GetActiveSupplierList();
        Task<Supplier> GetSupplier(int supplierId);
        Task<Supplier> AddSupplier(Supplier supplier);
        Task<Supplier> UpdateSupplier(Supplier supplier);
        Task<Supplier> DeleteSupplier(Supplier supplier);

        Task<List<ReadInBill>> GetActiveReadInBillList();
        Task<List<ReadInBill>> GetActiveReadInBillBySupplierList(int id);
        Task<ReadInBill> GetReadInBill(int readInBillId);
        Task<ReadInBill> AddReadInBill(ReadInBill readInBill);
        Task<ReadInBill> UpdateReadInBill(ReadInBill readInBill);
        Task<ReadInBill> DeleteReadInBill(ReadInBill readInBill);
    }
}
