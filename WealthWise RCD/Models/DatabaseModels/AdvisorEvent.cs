using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class AdvisorEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AdvisorId { get; set; }
        public User Advisor { get; set; } = null!;
        public ICollection<User> Users { get; set; }
    }
}
