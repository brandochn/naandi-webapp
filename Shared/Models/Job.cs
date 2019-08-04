namespace Naandi.Shared.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string JobTitle { get; set; }
        public string OfficialHours { get; set; }
        public int YearsOfService { get; set; }
        public decimal Salary { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string ManagerName { get; set; }
        public string ManagerPosition { get; set; }
    }
}