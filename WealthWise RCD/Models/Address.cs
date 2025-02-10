using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetName {  get; set; }
        public string? ExtraInfo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        
    }
}
