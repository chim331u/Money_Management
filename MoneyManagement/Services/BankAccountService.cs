using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoneyManagement.AppContext;
using MoneyManagement.Contract;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.BankAccount;

namespace MoneyManagement.Services
{
    public class BankAccountService : IBankAccountService
    {

        private readonly ApplicationContext _context;
        private readonly ILogger<BankAccountService> _logger;

        public BankAccountService(ILogger<BankAccountService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;

        }

        #region Bank

        public async Task<ApiResponse<ICollection<BankMasterData>>> GetActiveBankList()
        {
            try
            {
                var result = await _context.BankMasterData.Include(x=>x.Country)
                    .Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();

                return new ApiResponse<ICollection<BankMasterData>>(result, "Active bank list retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve bank active list: {ex.Message}");
                return new ApiResponse<ICollection<BankMasterData>>(null, $"Error retrieving active bank list: {ex.Message}");
                
            }
        }

        public async Task<ApiResponse<BankMasterData>> GetBank(int bankId)
        {
            try
            {
                var result = await _context.BankMasterData
                    .Include(x=>x.Country)
                    .Where(x=>x.Id == bankId).FirstOrDefaultAsync();

                return new ApiResponse<BankMasterData>(result, $"Bank with ID {bankId} retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve bank {bankId}: {ex.Message}");
                return new ApiResponse<BankMasterData>(null, $"Error retrieving bank {bankId}: {ex.Message}");
            }
        }

        public async Task<ApiResponse<BankMasterData>> UpdateBank(BankMasterData? item)
        {
           try
            {
                var existingBank = await _context.BankMasterData.Include(x=>x.Country).Where(x=>x.Id==item.Id).FirstOrDefaultAsync();

                if (existingBank == null)
                {
                    _logger.LogWarning($"Bank to update not found");
                    return new ApiResponse<BankMasterData>(null, $"Bank with ID {item.Id} not found.");
                }
                
                existingBank.Address=item.Address;
                existingBank.City=item.City;
                existingBank.Name = item.Name;
                existingBank.Description = item.Description;
                existingBank.IsActive=item.IsActive;
                existingBank.Mail=item.Mail;
                existingBank.Phone=item.Phone;
                existingBank.ReferenceName=item.ReferenceName;
                existingBank.WebUrl=item.WebUrl;
                existingBank.LastUpdatedDate = DateTime.Now;
                existingBank.Note=item.Note;

                _context.BankMasterData.Update(existingBank);
                await _context.SaveChangesAsync();

                return new ApiResponse<BankMasterData>(existingBank, $"Bank with ID {item.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Updating bank: {ex.Message}");
                return new ApiResponse<BankMasterData>(null, $"Error updating bank: {ex.Message}");

            }

        }

        public async Task<ApiResponse<BankMasterData>> AddBank(BankMasterData? item)
        {
            var country = await _context.Country.FindAsync(item.Country.Id);
           
            if (country == null)
            {
                _logger.LogWarning($"Country not found");
                return new ApiResponse<BankMasterData>(null, $"Country not found.");
            }
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Country = country;

                await _context.BankMasterData.AddAsync(item);
                await _context.SaveChangesAsync();

                return new ApiResponse<BankMasterData>(await _context.BankMasterData.FindAsync(item.Id), $"Bank with ID {item.Id} added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding bank: {ex.Message}");
                return null;

            }

        }

        public async Task<ApiResponse<bool>> DeleteBank(int id)
        {
            try
            {
                var existingBank = await _context.BankMasterData.Include(x=>x.Country).Where(x=> x.Id == id).FirstOrDefaultAsync();

                if (existingBank == null)
                {
                    _logger.LogWarning($"Bank not found");
                    return new ApiResponse<bool>(false, $"Bank with ID {id} not found.");
                }

                existingBank.LastUpdatedDate = DateTime.Now;
                existingBank.IsActive = false;

                _context.BankMasterData.Update(existingBank);

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>(true, $"Bank with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting bank: {ex.Message}");
                return new ApiResponse<bool>(false, $"Error deleting bank: {ex.Message}");

            }

        }

        #endregion

        #region Account

        public async Task<ApiResponse<ICollection<AccountMasterData>>> GetActiveAccountList()
        {
            try
            {
                var result = await _context.AccountMasterData
                    .Include(x=>x.Currency)
                    .Include(x=>x.BankMasterData)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.CreatedDate).ToListAsync();
                
                return new ApiResponse<ICollection<AccountMasterData>>(result, "Active account list retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve Account list: {ex.Message}");
                return new ApiResponse<ICollection<AccountMasterData>>(null, $"Error retrieving active account list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AccountMasterData>> GetAccount(int accountId)
        {
            try
            {
                var result = await _context.AccountMasterData
                    .Include(x=>x.Currency)
                    .Include(x=>x.BankMasterData)
                    .Where(x=>x.Id==accountId).FirstOrDefaultAsync();
                
                return new ApiResponse<AccountMasterData>(result, $"Account with ID {accountId} retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve Account {accountId}: {ex.Message}");
                return new ApiResponse<AccountMasterData>(null, $"Error retrieve Account {accountId}: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AccountMasterData>> UpdateAccount(AccountMasterData? item)
        {
            try
            {
                var existingAccount = await _context.AccountMasterData
                    .Include(x=>x.Currency)
                    .Include(x=>x.BankMasterData).Where(x=>x.Id==item.Id).FirstOrDefaultAsync();

                if (existingAccount == null)
                {
                    _logger.LogWarning($"Account to update not found");
                    return new ApiResponse<AccountMasterData>(null, $"Account with ID {item.Id} not found.");
                }
                
                existingAccount.AccountType = item.AccountType;
                existingAccount.LastUpdatedDate = DateTime.Now;

                _context.AccountMasterData.Update(item);
                await _context.SaveChangesAsync();

                return new ApiResponse<AccountMasterData>(existingAccount, $"Account with ID {item.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ApiResponse<AccountMasterData>(null, $"Error updating account: {ex.Message}");

            }

        }

        public async Task<ApiResponse<AccountMasterData>> AddAccount(AccountMasterData? item)
        {
            if (item == null)
            {
                _logger.LogWarning($"Account to add not found");
                return new ApiResponse<AccountMasterData>(null, "Account to add not found.");

            }

            if (item.Currency == null)
            {
                _logger.LogWarning($"Currency for Account to add not found");
                return new ApiResponse<AccountMasterData>(null, "Currency for Account to add not found.");
                    
            }

            var currency = await _context.Currency.FindAsync(item.Currency.Id);
            if (item.BankMasterData == null)
            {
                _logger.LogWarning($"Bank for Account to add not found");
                return new ApiResponse<AccountMasterData>(null, "Bank for Account to add not found.");
            }

            var bank = await _context.BankMasterData.FindAsync(item.BankMasterData.Id);

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.BankMasterData = bank;
                item.Currency = currency;

                await _context.AccountMasterData.AddAsync(item);
                await _context.SaveChangesAsync();

                return new ApiResponse<AccountMasterData>(await _context.AccountMasterData.FindAsync(item.Id), $"Account with ID {item.Id} added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding new account : {ex.Message}");
                return new ApiResponse<AccountMasterData>(null, $"Error adding new account : {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAccount(int id)
        {
            try
            {
                var item = await _context.AccountMasterData
                    .Include(x=>x.Currency)
                    .Include(x=>x.BankMasterData)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                
                if (item == null)
                {
                    _logger.LogWarning($"Account not found");
                    return new ApiResponse<bool>(false, $"Account with ID {id} not found.");
                }
                
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                _context.AccountMasterData.Update(item);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>(true, $"Account with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting new account : {ex.Message}");
                return new ApiResponse<bool>(false, $"Error deleting new account : {ex.Message}");

            }

        }
        #endregion

    }
}