using MoneyManagement.Contract;
using MoneyManagement.Models.Salary;

namespace MoneyManagement.Interfaces;

public interface ISalaryService
{
    Task<ApiResponse<ICollection<Salary>>> GetActiveSalaryList();
    Task<ApiResponse<Salary>> GetSalary(int salaryId);
    Task<ApiResponse<Salary>> AddSalary(Salary salary);
    Task<ApiResponse<Salary>> UpdateSalary(Salary salary);
    Task<ApiResponse<bool>> DeleteSalary(int id);

}