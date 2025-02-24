using System.ComponentModel.DataAnnotations;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Certificate
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Type { get; set; }
        public ICollection<User> Advisors { get; set; }
    }
}
