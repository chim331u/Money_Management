using MoneyManagement_Web.Data.Salary;

namespace MoneyManagement_Web.Interfaces
{
    public interface ITestServices
    {

        Task<string> UploadFile(MultipartFormDataContent item);
        Task<string> PostFileSalary(SalaryWithFile salaryFile);
    }
}
