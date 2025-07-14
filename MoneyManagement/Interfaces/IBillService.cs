using MoneyManagement.Contract;
using MoneyManagement.Models.Bill;

namespace MoneyManagement.Interfaces
{
    public interface IBillService
    {
        Task<ApiResponse<ICollection<Bill>>> GetActiveBillList();
        Task<ApiResponse<Bill>> GetBill(int BillId);
        Task<ApiResponse<Bill>> AddBill(Bill bill);
        Task<ApiResponse<Bill>> UpdateBill(Bill bill);
        Task<ApiResponse<bool>> DeleteBill(int id);
    }
}
