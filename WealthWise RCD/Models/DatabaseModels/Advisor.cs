using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Advisor: IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Biography { get; set; }
        // image???
        public ICollection<AdvisorCertificate> Certificates { get; set; }
    }
}
