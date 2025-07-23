using MoneyManagement_Api.Models.Utility;

namespace MoneyManagement_Api.Models.AncillaryData;

public class ServiceUser : BaseEntity
{
    public int Id { get; set; }


    public string? Name { get; set; }
    public string? Surname { get; set; }
}