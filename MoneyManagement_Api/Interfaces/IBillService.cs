using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.Bill;

namespace MoneyManagement_Api.Interfaces;

public interface IBillService
{
    Task<ApiResponse<ICollection<Bill>>> GetActiveBillList();
    Task<ApiResponse<Bill>> GetBill(int BillId);
    Task<ApiResponse<Bill>> AddBill(Bill bill);
    Task<ApiResponse<Bill>> UpdateBill(Bill bill);
    Task<ApiResponse<bool>> DeleteBill(int id);
}