using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.AncillaryData;

namespace MoneyManagement_Api.Interfaces;

public interface IAncillaryService
{
    Task<ApiResponse<ICollection<Country>>> GetActiveCountryList();
    Task<ApiResponse<Country>> GetCountry(int countryId);
    Task<ApiResponse<Country>> AddCountry(Country country);
    Task<ApiResponse<Country>> UpdateCountry(Country country);
    Task<ApiResponse<bool>> DeleteCountry(int id);

    Task<ApiResponse<ICollection<Currency>>> GetActiveCurrencyList();
    Task<ApiResponse<Currency>> GetCurrency(int currencyId);
    Task<ApiResponse<Currency>> AddCurrency(Currency currency);
    Task<ApiResponse<Currency>> UpdateCurrency(Currency currency);
    Task<ApiResponse<bool>> DeleteCurrency(int id);

    Task<ApiResponse<ICollection<CurrencyConversionRate>>> GetActiveCurrencyConversionList();
    Task<ApiResponse<CurrencyConversionRate>> GetCurrencyConversion(int currencyConversionRateId);
    Task<ApiResponse<CurrencyConversionRate>> GetCurrencyRate(string currencyALF3);
    Task<ApiResponse<int>> UpdateCurrencyRate();
    Task<ApiResponse<int>> UpdateAllCurrencyRate();
    Task<ApiResponse<string>> ClearUnusedRates();
    Task<ApiResponse<CurrencyConversionRate>> AddCurrencyConversion(CurrencyConversionRate currencyConversionRate);
    Task<ApiResponse<CurrencyConversionRate>> UpdateCurrencyConversion(CurrencyConversionRate currencyConversionRate);
    Task<ApiResponse<bool>> DeleteCurrencyConversion(int id);


    Task<ApiResponse<ICollection<ServiceUser>>> GetActiveServiceUserList();
    Task<ApiResponse<ServiceUser>> GetServiceUser(int serviceUserId);
    Task<ApiResponse<ServiceUser>> AddServiceUser(ServiceUser serviceUser);
    Task<ApiResponse<ServiceUser>> UpdateServiceUser(ServiceUser serviceUser);
    Task<ApiResponse<bool>> DeleteServiceUser(int id);

    Task<ApiResponse<ICollection<Supplier>>> GetActiveSupplierList();
    Task<ApiResponse<Supplier>> GetSupplier(int supplierId);
    Task<ApiResponse<Supplier>> AddSupplier(Supplier supplier);
    Task<ApiResponse<Supplier>> UpdateSupplier(Supplier supplier);
    Task<ApiResponse<bool>> DeleteSupplier(int id);

    Task<ApiResponse<ICollection<ReadInBill>>> GetActiveReadInBillList();
    Task<ApiResponse<ICollection<ReadInBill>>> GetActiveReadInBillBySupplierList(int id);
    Task<ApiResponse<ReadInBill>> GetReadInBill(int readInBillId);
    Task<ApiResponse<ReadInBill>> AddReadInBill(ReadInBill readInBill);
    Task<ApiResponse<ReadInBill>> UpdateReadInBill(ReadInBill readInBill);
    Task<ApiResponse<bool>> DeleteReadInBill(int id);
}