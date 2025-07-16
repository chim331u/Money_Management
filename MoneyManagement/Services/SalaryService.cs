using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Contract;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Salary;

namespace MoneyManagement.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ApplicationContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IAncillaryService _anchillaryService;
        private readonly ILogger<SalaryService> _logger;

        public SalaryService(ILogger<SalaryService> logger, ApplicationContext context, IUtilityService utilityService,
            IAncillaryService anchillaryService)
        {
            _context = context;
            _utilityService = utilityService;
            _anchillaryService = anchillaryService;
            _logger = logger;
        }

        #region Salary

        public async Task<ApiResponse<ICollection<Salary>>> GetActiveSalaryList()
        {
            try
            {
                var result = await _context.Salary
                    .Include(c => c.Currency)
                    .Include(c => c.User)
                    .Where(x => x.IsActive).OrderByDescending(x => x.SalaryDate).ToListAsync();
                return new ApiResponse<ICollection<Salary>>(result, $"Active salary list fetched successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching active salary list: {ex.Message}");
                return new ApiResponse<ICollection<Salary>>(null, $"Error fetching active salary list: {ex.Message}");
                
            }
        }

        public async Task<ApiResponse<Salary>> GetSalary(int salaryId)
        {
            try
            {
                var result = await _context.Salary.Include(c => c.Currency).Include(c => c.User)
                    .Where(x => x.Id == salaryId).FirstOrDefaultAsync();
                return new ApiResponse<Salary>(result, $"Salary with ID {salaryId} fetched successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching salary with ID {salaryId}: {ex.Message}");
                return new ApiResponse<Salary>(null, $"Error fetching salary with ID {salaryId}: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Salary>> UpdateSalary(Salary item)
        {
            try
            {
                var existingSalary = await _context.Salary
                    .Include(c => c.Currency)
                    .Include(c => c.User)
                    .Where(x => x.Id == item.Id).FirstOrDefaultAsync();

                if (existingSalary == null)
                {
                    _logger.LogWarning($"Salary with ID {item.Id} not found for update.");
                    return new ApiResponse<Salary>(null, $"Salary with ID {item.Id} not found for update.");
                }

                existingSalary.SalaryValueEur = item.SalaryValue / (double)item.ExcengeRate;
                existingSalary.LastUpdatedDate = DateTime.Now;
                existingSalary.ExcengeRate = item.ExcengeRate;
                existingSalary.SalaryValue = item.SalaryValue;
                existingSalary.SalaryDate = item.SalaryDate;
                existingSalary.FileName = item.FileName;
                existingSalary.ReferMonth = item.ReferMonth;
                existingSalary.ReferYear = item.ReferYear;
                existingSalary.Note = item.Note;

                _context.Salary.Update(existingSalary);
                await _context.SaveChangesAsync();

                return new ApiResponse<Salary>(existingSalary, $"Salary with ID {item.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating salary: {ex.Message}");
                return new ApiResponse<Salary>(null, $"Error updating salary: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Salary>> AddSalary(Salary item)
        {
            try
            {
                var currency = await _anchillaryService.GetCurrency(item.Currency.Id);
                var user = await _anchillaryService.GetServiceUser(item.User.Id);
                var currRate = await _anchillaryService.GetCurrencyRate(item.Currency.CurrencyCodeALF3);

                item.ExcengeRate = 1;
                if (currRate != null)
                {
                    item.ExcengeRate = currRate.Data.RateValue;
                }

                item.Currency = currency.Data;
                item.User = user.Data;
                item.SalaryValueEur = item.SalaryValue / (double)item.ExcengeRate;

                item.LastUpdatedDate = DateTime.Now;
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                await _context.Salary.AddAsync(item);
                await _context.SaveChangesAsync();

                return new ApiResponse<Salary>(await _context.Salary.FindAsync(item.Id), $"Salary added successfully with ID {item.Id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding salary: {ex.Message}");
                return new ApiResponse<Salary>(null, $"Error adding salary: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteSalary(int id)
        {
            try
            {
                var existingSalary = await _context.Salary
                    .Include(c => c.Currency)
                    .Include(c => c.User)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (existingSalary == null)
                {
                    _logger.LogWarning($"Salary with ID {id} not found for deletion.");
                    return new ApiResponse<bool>(true, $"Salary with ID {id} not found for deletion.");
                }
                
                existingSalary.LastUpdatedDate = DateTime.Now;
                existingSalary.IsActive = false;

                _context.Salary.Update(existingSalary);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>(true, $"Salary with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting salary: {ex.Message}");
                return new ApiResponse<bool>(false, $"Error deleting salary: {ex.Message}");
            }
        }

        #endregion
    }
}