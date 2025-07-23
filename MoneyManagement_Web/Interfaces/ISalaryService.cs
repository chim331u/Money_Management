using MoneyManagement_Web.Data.Salary;

namespace MoneyManagement_Web.Interfaces
{
    public interface ISalaryService
    {
        Task<List<Salary>> GetActiveSalaryList();
        Task<Salary> GetSalary(int salaryId);
        Task<Salary> AddSalary(Salary salary);
        Task<Salary> UpdateSalary(Salary salary);
        Task<Salary> DeleteSalary(Salary salary);


    }
}
