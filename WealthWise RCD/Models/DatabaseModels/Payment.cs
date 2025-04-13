using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public enum PaymentType
    {
        CreditCard = 0,
        PayPal = 1
    }

    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public PaymentType Type { get; set; }
        #region CC Info
        [StringLength(19)]
        public string? CardNumber {  get; set; }
        [MaxLength(255)]
        public string? CardholderName {  get; set; }
        public DateTime? ExpDate { get; set; }

        [StringLength(3)]
        public string? Cvc { get; set; }
        #endregion
        #region PayPal Info
        [MaxLength(255)]
        public string? AccountName {  get; set; }
        #endregion
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}
