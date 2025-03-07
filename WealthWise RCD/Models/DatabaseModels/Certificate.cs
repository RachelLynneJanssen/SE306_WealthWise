using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Certificate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Type { get; set; }
        public ICollection<ApplicationUser> Advisors { get; set; }
    }
}
