using MoneyManagement_Web.Data.Bill;

namespace MoneyManagement_Web.Interfaces
{
    public interface IBillService
    {
        Task<List<Bill>> GetActiveBillList();
        Task<Bill> GetBill(int billId);
        Task<Bill> AddBill(Bill bill);
        Task<Bill> UpdateBill(Bill bill);    
        Task<Bill> DeleteBill(Bill bill);
        Task<string> UploadFile(MultipartFormDataContent item, int id);
    }
}
