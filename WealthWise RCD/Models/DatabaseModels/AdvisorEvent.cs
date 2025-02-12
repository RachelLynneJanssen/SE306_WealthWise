using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class AdvisorEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int AdvisorId { get; set; }
        [ForeignKey(nameof(AdvisorId))]
        public Advisor Advisor { get; set; } = null!;
        public ICollection<User> Users { get; set; }
    }
}
