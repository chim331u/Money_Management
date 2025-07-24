using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.Salary;
using MoneyManagement_Data;

namespace MoneyManagement_Api.Interfaces;

public interface ISalaryService
{
    Task<ApiResponse<ICollection<Salary>>> GetActiveSalaryList();
    Task<ApiResponse<Salary>> GetSalary(int salaryId);
    Task<ApiResponse<Salary>> AddSalary(Salary salary);
    Task<ApiResponse<Salary>> UpdateSalary(Salary salary);
    Task<ApiResponse<bool>> DeleteSalary(int id);
}