using System.Globalization;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using MoneyManagement_Api.AppContext;
using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.AncillaryData;
using MoneyManagement_Data;

namespace MoneyManagement_Api.Services;

public class AncillaryService : IAncillaryService
{
    private readonly ApplicationContext _context;
    private readonly IUtilityService _utilityService;
    private readonly ILogger<AncillaryService> _logger;

    public AncillaryService(ILogger<AncillaryService> logger, ApplicationContext context,
        IUtilityService utilityService)
    {
        _logger = logger;
        _context = context;
        _utilityService = utilityService;
        //
        // Task.Run(async () =>
        // {
        //     await
        //TODO scheduile this task
        UpdateCurrencyRate();
        //     _logger.LogInformation("Update currency rate Async task completed.");
        // });
    }

    #region Country *

    public async Task<ApiResponse<ICollection<Country>>> GetActiveCountryList()
    {
        try
        {
            var result = await _context.Country.Where(x => x.IsActive).ToListAsync();
            return new ApiResponse<ICollection<Country>>(result, "Success");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting active country list {ex.Message}");
            return new ApiResponse<ICollection<Country>>(null, $"Error getting active country list {ex.Message}");
        }
    }

    public async Task<ApiResponse<Country>> GetCountry(int countryId)
    {
        try
        {
            var result = await _context.Country.FindAsync(countryId);
            return new ApiResponse<Country>(result, "Success");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching country {ex.Message}");
            return new ApiResponse<Country>(null, $"Error fetching country {ex.Message}");
        }
    }

    public async Task<ApiResponse<Country>> UpdateCountry(Country item)
    {
        try
        {
            var existingCountry = await _context.Country.FindAsync(item.Id);

            if (existingCountry == null)
            {
                _logger.LogWarning($"Country with ID {item.Id} not found.");
                return new ApiResponse<Country>(item, $"Country with ID {item.Id} not found");
            }

            // Update the properties of the existing country
            existingCountry.Name = item.Name;
            existingCountry.CountryCodeNum3 = item.CountryCodeNum3;
            existingCountry.CountryCodeALF3 = item.CountryCodeALF3;
            existingCountry.IsActive = item.IsActive;
            existingCountry.LastUpdatedDate = DateTime.Now;
            existingCountry.Description = item.Description;
            existingCountry.Note = item.Note;

            _context.Country.Update(existingCountry);
            await _context.SaveChangesAsync();

            return new ApiResponse<Country>(existingCountry, "Success");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating country {ex.Message}");
            return new ApiResponse<Country>(null, $"Error updating country {ex.Message}");
        }
    }

    public async Task<ApiResponse<Country>> AddCountry(Country item)
    {
        try
        {
            if (item == null)
            {
                _logger.LogWarning($"Country to add is null");
                return new ApiResponse<Country>(null, "Country to add is null");
            }

            item.CreatedDate = DateTime.Now;
            item.IsActive = true;

            await _context.Country.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<Country>(await _context.Country.FindAsync(item.Id), "Success");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding new Country: {ex.Message}");
            return new ApiResponse<Country>(null, $"Error adding new Country: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteCountry(int id)
    {
        try
        {
            var existingCountry = await _context.Country.FindAsync(id);

            if (existingCountry == null)
            {
                _logger.LogWarning($"Country with ID {id} not found.");
                return new ApiResponse<bool>(false, "Country with ID not found.");
            }

            existingCountry.LastUpdatedDate = DateTime.Now;
            existingCountry.IsActive = false;

            _context.Country.Update(existingCountry);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Country deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new ApiResponse<bool>(false, $"Error deleting country {ex.Message}");
        }
    }

    #endregion

    #region Currency *

    public async Task<ApiResponse<ICollection<Currency>>> GetActiveCurrencyList()
    {
        try
        {
            var result = await _context.Currency.Where(x => x!.IsActive).ToListAsync();
            return new ApiResponse<ICollection<Currency>>(result, $"Currency list fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching active currency list {ex.Message}");
            return new ApiResponse<ICollection<Currency>>(null, $"Error getting active currency list {ex.Message}");
        }
    }

    public async Task<ApiResponse<Currency>> GetCurrency(int id)
    {
        try
        {
            var result = await _context.Currency.FindAsync(id);
            return new ApiResponse<Currency>(result, $"Currency fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching active currency {ex.Message}");
            return new ApiResponse<Currency>(null, $"Error getting active currency {ex.Message}");
        }
    }

    public async Task<ApiResponse<Currency>> UpdateCurrency(Currency item)
    {
        try
        {
            var existingCurrency = await _context.Currency.FindAsync(item.Id);

            if (existingCurrency == null)
            {
                _logger.LogWarning($"Currency with ID {item.Id} not found.");
                return new ApiResponse<Currency>(item, $"Currency with ID {item.Id} not found");
            }

            // Update the properties of the existing currency
            existingCurrency.Name = item.Name;
            existingCurrency.CurrencyCodeNum3 = item.CurrencyCodeNum3;
            existingCurrency.CurrencyCodeALF3 = item.CurrencyCodeALF3;
            existingCurrency.IsActive = item.IsActive;
            existingCurrency.LastUpdatedDate = DateTime.Now;
            existingCurrency.Description = item.Description;
            existingCurrency.Note = item.Note;

            _context.Currency.Update(existingCurrency);
            await _context.SaveChangesAsync();

            return new ApiResponse<Currency>(existingCurrency, $"Currency updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating Currency: {ex.Message}");
            return new ApiResponse<Currency>(null, $"Error updating Currency: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Currency>> AddCurrency(Currency item)
    {
        try
        {
            if (item == null)
            {
                _logger.LogWarning($"Currency to add is null");
                return new ApiResponse<Currency>(null, "Currency to add is null");
            }

            item.CreatedDate = DateTime.Now;
            item.IsActive = true;

            await _context.Currency.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<Currency>(await _context.Currency.FindAsync(item.Id), "Currency added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding new Currency: {ex.Message} ");
            return new ApiResponse<Currency>(null, $"Error adding new Currency: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteCurrency(int id)
    {
        try
        {
            var existingCurrency = await _context.Currency.FindAsync(id);

            if (existingCurrency == null)
            {
                _logger.LogWarning($"Currency with ID {id} not found.");
                return new ApiResponse<bool>(false, "Currency with ID not found.");
            }

            existingCurrency.LastUpdatedDate = DateTime.Now;
            existingCurrency.IsActive = false;

            _context.Currency.Update(existingCurrency);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Currency deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting currency {ex.Message}");
            return new ApiResponse<bool>(false, $"Error deleting currency {ex.Message}");
        }
    }

    #endregion

    #region Currency Conversion Rate *

    public async Task<ApiResponse<ICollection<CurrencyConversionRate>>> GetActiveCurrencyConversionList()
    {
        try
        {
            var result = await _context.CurrencyConversionRates.Where(x => x.IsActive)
                .OrderByDescending(x => x.ReferringDate).ToListAsync();
            return new ApiResponse<ICollection<CurrencyConversionRate>>(result,
                $"Currency conversion list fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching Currency conversion list: {ex.Message}");
            return new ApiResponse<ICollection<CurrencyConversionRate>>(null,
                $"Error getting active currency conversion list {ex.Message}");
        }
    }

    public async Task<ApiResponse<CurrencyConversionRate>> GetCurrencyConversion(int id)
    {
        try
        {
            var result = await _context.CurrencyConversionRates.FindAsync(id);
            return new ApiResponse<CurrencyConversionRate>(result, $"Currency conversion list fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching currency conversion: {ex.Message}");
            return new ApiResponse<CurrencyConversionRate>(null,
                $"Error getting currency conversion list {ex.Message}");
        }
    }

    public async Task<ApiResponse<CurrencyConversionRate>> GetCurrencyRate(string currencyAlf3)
    {
        try
        {
            var result = await _context.CurrencyConversionRates.Where(c =>
                    c.IsActive == true && c.CurrencyCodeALF3.ToUpper() == currencyAlf3.ToUpper())
                .OrderByDescending(c => c.ReferringDate)
                .FirstOrDefaultAsync();

            return new ApiResponse<CurrencyConversionRate>(result, $"Currency conversion list fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching currency rate: {ex.Message}");
            return new ApiResponse<CurrencyConversionRate>(null, $"Error getting currency rate: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CurrencyConversionRate>> UpdateCurrencyConversion(CurrencyConversionRate item)
    {
        try
        {
            var existingRate = await _context.CurrencyConversionRates.FindAsync(item.Id);
            if (existingRate == null)
            {
                _logger.LogWarning($"CurrencyConversionRate with ID {item.Id} not found.");
                return new ApiResponse<CurrencyConversionRate>(null, "CurrencyConversionRate with ID not found.");
            }

            // Update the properties of the existing rate
            existingRate.RateValue = item.RateValue;
            existingRate.CurrencyCodeALF3 = item.CurrencyCodeALF3;
            existingRate.IsActive = item.IsActive;
            existingRate.LastUpdatedDate = DateTime.Now;
            existingRate.ReferringDate = item.ReferringDate;
            existingRate.UniqueKey = item.UniqueKey;
            existingRate.Note = item.Note;

            _context.CurrencyConversionRates.Update(existingRate);
            await _context.SaveChangesAsync();

            return new ApiResponse<CurrencyConversionRate>(existingRate,
                $"CurrencyConversionRate updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating currency conversion: {ex.Message}");
            return new ApiResponse<CurrencyConversionRate>(null, $"Error updating currency conversion {ex.Message}");
        }
    }

    public async Task<ApiResponse<CurrencyConversionRate>> AddCurrencyConversion(CurrencyConversionRate item)
    {
        try
        {
            item.CreatedDate = DateTime.Now;
            item.IsActive = true;

            await _context.CurrencyConversionRates.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<CurrencyConversionRate>(await _context.CurrencyConversionRates.FindAsync(item.Id),
                $"Currency conversion rate added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding new Currency Conversion Rate: {ex.Message}");
            return new ApiResponse<CurrencyConversionRate>(null,
                $"Error adding new Currency Conversion Rate: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteCurrencyConversion(int id)
    {
        try
        {
            var existingRate = await _context.CurrencyConversionRates.FindAsync(id);

            if (existingRate == null)
            {
                _logger.LogWarning($"CurrencyConversionRate with ID {id} not found.");
                return new ApiResponse<bool>(false, "CurrencyConversionRate with ID not found.");
            }

            existingRate.LastUpdatedDate = DateTime.Now;
            existingRate.IsActive = false;

            _context.CurrencyConversionRates.Update(existingRate);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Currency conversion rate deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting currency conversion rate: {ex.Message}");
            return new ApiResponse<bool>(false, $"Error deleting currency conversion rate: {ex.Message}");
        }
    }

    private async Task<DateTime> GetLastUpdateDate()
    {
        try
        {
            var lastDateUpdate = await _context.CurrencyConversionRates.Where(c => c.IsActive == true)
                .OrderByDescending(c => c.ReferringDate).Select(c => c.ReferringDate)
                .FirstOrDefaultAsync();

            return lastDateUpdate;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching currency rate / last update date: {ex.Message}");
            return DateTime.MinValue;
        }
    }

    public async Task<ApiResponse<int>> UpdateCurrencyRate()
    {
        var _startTime = DateTime.Now;
        _logger.LogInformation($"Start Update Currency Conversion Rates ... ");

        if (await GetLastUpdateDate() < DateTime.Now.Date)
        {
            var doc = new XmlDocument();
            doc.Load(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

            var nodes = doc.SelectNodes("//*[@currency]");

            if (nodes != null)
            {
                var currentCurrencies = await GetActiveCurrencyList();

                //int counter = 0;
                foreach (XmlNode node in nodes)
                {
                    var currency = node.Attributes["currency"].Value;

                    if (!currentCurrencies.Data.Any(x => x.CurrencyCodeALF3 == currency)) continue;
                    var rate = decimal.Parse(node.Attributes["rate"].Value, NumberStyles.Any, new CultureInfo("en-Us"));
                    var uniqueK = string.Concat(currency, rate.ToString(CultureInfo.InvariantCulture),
                        DateTime.Now.Date);

                    var isCodeExist =
                        _context.CurrencyConversionRates.Any(c => c.UniqueKey == uniqueK && c.IsActive == true);

                    if (!isCodeExist)
                        // await AddCurrencyConversion(new CurrencyConversionRate
                        // {
                        //     CreatedDate = DateTime.Now,
                        //     CurrencyCodeALF3 = currency,
                        //     IsActive = true,
                        //     LastUpdatedDate = DateTime.Now,
                        //     ReferringDate = DateTime.Now,
                        //     RateValue = rate,
                        //     UniqueKey = uniqueK
                        // });
                        await _context.CurrencyConversionRates.AddAsync(new CurrencyConversionRate
                        {
                            CreatedDate = DateTime.Now,
                            CurrencyCodeALF3 = currency,
                            IsActive = true,
                            LastUpdatedDate = DateTime.Now,
                            ReferringDate = DateTime.Now,
                            RateValue = rate,
                            UniqueKey = uniqueK
                        });
                }

                try
                {
                    var result = await _context.SaveChangesAsync();

                    _logger.LogInformation(
                        $"Currency rate updated in {_utilityService.TimeDiff(_startTime, DateTime.Now)}");
                    return new ApiResponse<int>(result, "Currency conversion rate updated successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error updating currency rate: {ex.Message}");
                    return new ApiResponse<int>(-2, $"Error updating currency rate: {ex.Message}");
                }
            }

            _logger.LogWarning($"Currency rates NOT updated {_utilityService.TimeDiff(_startTime, DateTime.Now)}");
            return new ApiResponse<int>(-1, "Currency conversion rate not updated successfully");
        }

        _logger.LogInformation($"Currency rates already updated {_utilityService.TimeDiff(_startTime, DateTime.Now)}");
        return new ApiResponse<int>(1, "Currency conversion rate already updated successfully");
    }

    //Deprecated
    //TODO delete method: duplicated
    public async Task<ApiResponse<int>> UpdateAllCurrencyRate()
    {
        if (await GetLastUpdateDate() < DateTime.Now.Date)
        {
            var doc = new XmlDocument();
            doc.Load(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

            var nodes = doc.SelectNodes("//*[@currency]");

            if (nodes != null)
            {
                //int counter = 0;
                foreach (XmlNode node in nodes)
                {
                    var currency = node.Attributes["currency"].Value;
                    var rate = decimal.Parse(node.Attributes["rate"].Value, NumberStyles.Any, new CultureInfo("en-Us"));
                    var uniqueK = string.Concat(currency, rate.ToString(CultureInfo.InvariantCulture),
                        DateTime.Now.Date);

                    var isCodeExist =
                        _context.CurrencyConversionRates.Any(c => c.UniqueKey == uniqueK && c.IsActive == true);

                    if (!isCodeExist)
                        //add new entry
                        await AddCurrencyConversion(new CurrencyConversionRate
                        {
                            CreatedDate = DateTime.Now,
                            CurrencyCodeALF3 = currency,
                            IsActive = true,
                            LastUpdatedDate = DateTime.Now,
                            ReferringDate = DateTime.Now,
                            RateValue = rate,
                            UniqueKey = uniqueK
                        });
                }

                try
                {
                    var result = await _context.SaveChangesAsync();
                    _logger.LogInformation("Currency rate updated");
                    return new ApiResponse<int>(result, "Currency conversion rate updated successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new ApiResponse<int>(-2,
                        $"Currency conversion rate not updated successfully: {ex.Message} ");
                }
            }

            _logger.LogWarning("Currency rate NOT updated");
            return new ApiResponse<int>(-1, "Currency conversion rate not updated successfully");
        }
        else
        {
            _logger.LogInformation("Currency rate already updated");
            return new ApiResponse<int>(1, "Currency conversion rate already updated successfully");
        }
    }

    public async Task<ApiResponse<string>> ClearUnusedRates()
    {
        var currentCurrencies =
            await _context.Currency.Where(x => x.IsActive).Select(x => x.CurrencyCodeALF3).ToListAsync();

        var currentCurrencyRates = await GetActiveCurrencyConversionList();

        var rejectList = currentCurrencyRates.Data.Where(i => currentCurrencies.Contains(i.CurrencyCodeALF3));
        var filteredList = currentCurrencyRates.Data.Except(rejectList);
        try
        {
            foreach (var item in filteredList)
            {
                //await DeleteCurrencyConversion(item);
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;
                _context.CurrencyConversionRates.Update(item);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Clear Currency rate completed");
            return new ApiResponse<string>("Deleted unused currency rates",
                "Clear Currency rate completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error clearing currency rates: {ex.Message}");
            return new ApiResponse<string>($"Error clearing currency rates: {ex.Message}",
                $"Error clearing currency rates: {ex.Message}");
        }
    }

    #endregion

    #region Service User *

    public async Task<ApiResponse<ICollection<ServiceUser>>> GetActiveServiceUserList()
    {
        try
        {
            var result = await _context.ServiceUser.Where(x => x.IsActive).OrderBy(x => x.CreatedDate).ToListAsync();
            return new ApiResponse<ICollection<ServiceUser>>(result, "Service user list fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching  active serviceUser list: {ex.Message}");
            return new ApiResponse<ICollection<ServiceUser>>(null,
                $"Error fetching active serviceUser list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ServiceUser>> GetServiceUser(int serviceUserId)
    {
        try
        {
            var result = await _context.ServiceUser.FindAsync(serviceUserId);
            return new ApiResponse<ServiceUser>(result, "Service user fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error  fetching serviceUser: {ex.Message}");
            return new ApiResponse<ServiceUser>(null, $"Error fetching serviceUser: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ServiceUser>> UpdateServiceUser(ServiceUser item)
    {
        try
        {
            var existingUser = await _context.ServiceUser.FindAsync(item.Id);
            if (existingUser == null)
            {
                _logger.LogWarning($"ServiceUser with ID {item.Id} not found.");
                return new ApiResponse<ServiceUser>(null, "Service user not found");
            }

            // Update the properties of the existing user
            existingUser.Name = item.Name;
            existingUser.Surname = item.Surname;
            existingUser.IsActive = item.IsActive;
            existingUser.Note = item.Note;
            existingUser.LastUpdatedDate = DateTime.Now;

            _context.ServiceUser.Update(existingUser);

            await _context.SaveChangesAsync();

            return new ApiResponse<ServiceUser>(existingUser, "Service user updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating  serviceUser: {ex.Message}");
            return new ApiResponse<ServiceUser>(null, $"Service user not updated {ex.Message}");
        }
    }

    public async Task<ApiResponse<ServiceUser>> AddServiceUser(ServiceUser item)
    {
        try
        {
            item.CreatedDate = DateTime.Now;
            item.IsActive = true;
            item.LastUpdatedDate = DateTime.Now;

            await _context.ServiceUser.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<ServiceUser>(await _context.ServiceUser.FindAsync(item.Id),
                "Service user added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding serviceUser: {ex.Message}");
            return new ApiResponse<ServiceUser>(null, $"Service user not added {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteServiceUser(int id)
    {
        try
        {
            var existingUser = await _context.ServiceUser.FindAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning($"ServiceUser with ID {id} not found.");
                return new ApiResponse<bool>(false, "Service user not found");
            }

            existingUser.LastUpdatedDate = DateTime.Now;
            existingUser.IsActive = false;

            _context.ServiceUser.Update(existingUser);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Service user deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting serviceUser: {ex.Message}");
            return new ApiResponse<bool>(false, $"Error deleting serviceUser {ex.Message}");
        }
    }

    #endregion

    #region Supplier *

    public async Task<ApiResponse<ICollection<Supplier>>> GetActiveSupplierList()
    {
        try
        {
            var result = await _context.suppliers.Where(x => x.IsActive).ToListAsync();
            return new ApiResponse<ICollection<Supplier>>(result, "Supplier list fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching Active supplier list: {ex.Message}");
            return new ApiResponse<ICollection<Supplier>>(null, $"Error fetching Active supplier list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Supplier>> GetSupplier(int supplierId)
    {
        try
        {
            var result = await _context.suppliers.FindAsync(supplierId);
            return new ApiResponse<Supplier>(result, "Supplier fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching supplier {ex.Message}");

            return new ApiResponse<Supplier>(null, $"Error fetching supplier {ex.Message}");
        }
    }

    public async Task<ApiResponse<Supplier>> UpdateSupplier(Supplier item)
    {
        try
        {
            var existingSupplier = await _context.suppliers.FindAsync(item.Id);
            if (existingSupplier == null)
            {
                _logger.LogWarning($"Supplier with ID {item.Id} not found.");
                return new ApiResponse<Supplier>(null, "Supplier not found");
            }

            // Update the properties of the existing supplier
            existingSupplier.Name = item.Name;
            existingSupplier.IsActive = item.IsActive;
            existingSupplier.Note = item.Note;
            existingSupplier.Description = item.Description;
            existingSupplier.Contract = item.Contract;
            existingSupplier.LastUpdatedDate = DateTime.Now;
            existingSupplier.UnitMeasure = item.UnitMeasure;
            existingSupplier.Type = item.Type;

            _context.suppliers.Update(existingSupplier);
            await _context.SaveChangesAsync();

            return new ApiResponse<Supplier>(existingSupplier, "Supplier updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating  supplier {ex.Message}");
            return new ApiResponse<Supplier>(null, $"Supplier not updated {ex.Message}");
        }
    }

    public async Task<ApiResponse<Supplier>> AddSupplier(Supplier item)
    {
        try
        {
            item.CreatedDate = DateTime.Now;
            item.IsActive = true;

            await _context.suppliers.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<Supplier>(await _context.suppliers.FindAsync(item.Id),
                "Supplier added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding supplier {ex.Message}");
            return new ApiResponse<Supplier>(null, $"Supplier not added {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteSupplier(int id)
    {
        try
        {
            var existingSupplier = await _context.suppliers.FindAsync(id);
            if (existingSupplier == null)
            {
                _logger.LogWarning($"Supplier with ID {id} not found.");
                return new ApiResponse<bool>(false, "Supplier not found");
            }

            existingSupplier.LastUpdatedDate = DateTime.Now;
            existingSupplier.IsActive = false;

            _context.suppliers.Update(existingSupplier);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Supplier deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting supplier {ex.Message}");
            return new ApiResponse<bool>(false, $"Supplier not deleted {ex.Message}");
        }
    }

    #endregion

    #region ReadInBill *

    public async Task<ApiResponse<ICollection<ReadInBill>>> GetActiveReadInBillList()
    {
        try
        {
            var result = await _context.readInBill.Include(c => c.Supplier).Where(x => x.IsActive).ToListAsync();
            return new ApiResponse<ICollection<ReadInBill>>(result, "ReadInBill fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Active readinbill list: {ex.Message}");
            return new ApiResponse<ICollection<ReadInBill>>(null,
                $"Error fetching Active readinbill list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ICollection<ReadInBill>>> GetActiveReadInBillBySupplierList(int id)
    {
        try
        {
            var result = await _context.readInBill.Include(c => c.Supplier)
                .Where(x => x.IsActive && x.Supplier.Id == id).ToListAsync();
            return new ApiResponse<ICollection<ReadInBill>>(result, "ReadInBill by supplier fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Active readinbill list by suppllier: {ex.Message}");
            return new ApiResponse<ICollection<ReadInBill>>(null,
                $"Error fetching Active readinbill by supplier list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ReadInBill>> GetReadInBill(int readInBillId)
    {
        try
        {
            var result = await _context.readInBill.Include(c => c.Supplier).Where(x => x.Id == readInBillId)
                .FirstOrDefaultAsync();
            return new ApiResponse<ReadInBill>(result, "ReadInBill fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Active readinbill {ex.Message}");
            return new ApiResponse<ReadInBill>(null, $"Error fetching Active readinbill {ex.Message}");
        }
    }

    public async Task<ApiResponse<ReadInBill>> UpdateReadInBill(ReadInBill item)
    {
        try
        {
            var supplier = await _context.suppliers.FindAsync(item.Supplier.Id);
            var existingBill = await _context.readInBill.FindAsync(item.Id);
            if (existingBill == null)
            {
                _logger.LogWarning($"ReadInBill with ID {item.Id} not found.");
                return new ApiResponse<ReadInBill>(null, "ReadInBill not found");
            }

            // Update the properties of the existing bill
            existingBill.BillProperty = item.BillProperty;
            existingBill.KeyWord = item.KeyWord;
            existingBill.PropertyDataType = item.PropertyDataType;
            existingBill.RegexString = item.RegexString;
            existingBill.Supplier = supplier;
            existingBill.LastUpdatedDate = DateTime.Now;
            existingBill.IsActive = item.IsActive;
            existingBill.Note = item.Note;

            _context.readInBill.Update(existingBill);
            await _context.SaveChangesAsync();

            return new ApiResponse<ReadInBill>(existingBill, "ReadInBill updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating read inbill {ex.Message}");
            return new ApiResponse<ReadInBill>(null, $"Error fetching Active readinbill {ex.Message}");
        }
    }

    public async Task<ApiResponse<ReadInBill>> AddReadInBill(ReadInBill item)
    {
        try
        {
            item.CreatedDate = DateTime.Now;
            item.IsActive = true;

            item.Supplier = await _context.suppliers.FindAsync(item.Supplier.Id);

            await _context.readInBill.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<ReadInBill>(await _context.readInBill.FindAsync(item.Id),
                "ReadInBill added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding readinbill {ex.Message}");
            return new ApiResponse<ReadInBill>(null, $"Error fetching ReadInBill {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteReadInBill(int id)
    {
        var item = await _context.readInBill.FindAsync(id);
        if (item == null)
        {
            _logger.LogWarning($"ReadInBill with ID {id} not found.");
            return new ApiResponse<bool>(false, "ReadInBill not found");
        }

        try
        {
            item.LastUpdatedDate = DateTime.Now;
            item.IsActive = false;

            _context.readInBill.Update(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, "ReadInBill deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting read inbill {ex.Message}");
            return new ApiResponse<bool>(false, "Error deleting ReadInBill");
        }
    }

    #endregion
}