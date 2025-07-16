using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Contract;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Bill;

namespace MoneyManagement.Services
{
    public class BillService : IBillService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BillService> _logger;

        public BillService(ApplicationContext context, ILogger<BillService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<ICollection<Bill>>> GetActiveBillList()
        {
            try
            {
                var result = await _context.bills.Include(c => c.Supplier)
                    .Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();
                
                if (result == null || result.Count == 0)
                {
                    _logger.LogWarning("No active bills found.");
                    return new ApiResponse<ICollection<Bill>>(null, "No active bills found.");
                }

                return new ApiResponse<ICollection<Bill>>(result, "Active bills retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Bill list {ex.Message}");
                return new ApiResponse<ICollection<Bill>>(null, $"Error retrieving Bill list: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Bill>> GetBill(int billId)
        {
            try
            {
                var result = await _context.bills.Include(c => c.Supplier).Where(x => x.Id == billId)
                    .FirstOrDefaultAsync();

                return new ApiResponse<Bill>(result, $"Bill retrieved successfully with ID: {billId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Bill {ex.Message}");
                return new ApiResponse<Bill>(null, $"Error retrieving Bill: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Bill>> UpdateBill(Bill item)
        {
            try
            {
                var existingBill = await _context.bills.Include(x => x.Supplier).Where(x => x.Id == item.Id)
                    .FirstOrDefaultAsync();
                if (existingBill == null)
                {
                    _logger.LogWarning("Bill not found for update.");
                    return new ApiResponse<Bill>(null, "Bill not found for update.");
                }

                existingBill.Amount = item.Amount;
                existingBill.BillNumber = item.BillNumber;
                existingBill.Consumption = item.Consumption;
                existingBill.FullPathFileName = item.FullPathFileName;
                existingBill.IsActive = item.IsActive;
                existingBill.PaidDate = item.PaidDate;
                existingBill.RefPeriodEnd = item.RefPeriodEnd;
                existingBill.RefPeriodStart = item.RefPeriodStart;
                existingBill.Supplier = item.Supplier;
                existingBill.Note = item.Note;
                existingBill.LastUpdatedDate = DateTime.Now;

                _context.bills.Update(existingBill);
                await _context.SaveChangesAsync();

                return new ApiResponse<Bill>(existingBill, "Bill updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Bill {ex.Message}");
                return new ApiResponse<Bill>(null, $"Error updating Bill: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Bill>> AddBill(Bill item)
        {
            var supplier = await _context.suppliers.Where(x => x.Id == item.Supplier.Id).FirstOrDefaultAsync();

            if (_context.bills.Where(x => x.BillNumber == item.BillNumber).Any())
            {
                _logger.LogWarning("Bill already present.");
                return new ApiResponse<Bill>(null, "Bill already present.");
            }

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Supplier = supplier;
                item.LastUpdatedDate = DateTime.Now;
                await _context.bills.AddAsync(item);
                await _context.SaveChangesAsync();

                return new ApiResponse<Bill>(await _context.bills.Include(x => x.Supplier).Where(x => x.Id == item.Id).FirstOrDefaultAsync(), $"Bill added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding Bill {ex.Message}");
                return null;
            }
        }

        public async Task<ApiResponse<bool>> DeleteBill(int id)
        {
            try
            {
                var existingBill = await _context.bills.Include(x => x.Supplier).Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (existingBill == null)
                {
                    _logger.LogWarning("Bill not found for deletion.");
                    return new ApiResponse<bool>(false, $"Bill not found for deletion.");
                }

                existingBill.LastUpdatedDate = DateTime.Now;
                existingBill.IsActive = false;

                _context.bills.Update(existingBill);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>(true, "Bill deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Bill {ex.Message}");
                return new ApiResponse<bool>(false, $"Error deleting Bill: {ex.Message}");
            }
        }
    }
}