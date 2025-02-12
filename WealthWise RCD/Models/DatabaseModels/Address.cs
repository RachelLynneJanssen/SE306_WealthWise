using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WealthWise_RCD.Models.DatabaseModels
{
    public enum AddressState
    {
        AL, AK, AZ, AR, CA, CO, CT, DE, FL, GA, HI, ID, IL, IN, IA, KS, KY,
        LA, ME, MD, MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND,
        OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY,
        AS,     // American Samoa
        DC,     // District of Colombia
        FM,     // Federated States of Micronesia
        GU,     // Guam
        MH,     // Marshal Islands
        MP,     // Norther Mariana Islands
        PW,     // Palau
        PR,     // Puerto Rico
        VI	// U.S. Virgin Islands

    }
    public class Address
    {
        public int Id { get; set; }
        [Required]
        public string StreetName {  get; set; }
        public string? ExtraInfo { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public AddressState State { get; set; }
        [Required, StringLength(5)]
        public string ZipCode {  get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
