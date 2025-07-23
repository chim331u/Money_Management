using System.ComponentModel.DataAnnotations;

namespace MoneyManagement_Web.Data.HouseThings
{
    public class HouseThingsRooms : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
    }
}
