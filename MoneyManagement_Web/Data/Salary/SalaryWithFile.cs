using Microsoft.AspNetCore.Components.Forms;

namespace MoneyManagement_Web.Data.Salary
{
    public class SalaryWithFile
    {
        //public MultipartFormDataContent? FileUpload { get; set; }
        //public Salary? Salary { get; set; }

        public IReadOnlyList<IBrowserFile>? FileUpload { get; set; }
        //public Salary? Salary{ get; set; }
        public int SalaryId { get; set; }
    }
}
