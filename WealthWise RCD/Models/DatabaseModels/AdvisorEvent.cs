using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class AdvisorEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Location { get; set; }
        public string AdvisorId { get; set; }
        public ApplicationUser Advisor { get; set; } = null!;
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
