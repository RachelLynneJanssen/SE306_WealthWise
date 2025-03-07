using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public enum PaymentType
    {
        CreditCard,
        PayPal
    }

    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public PaymentType Type { get; set; }
        #region CC Info
        [StringLength(16)]
        public string? CardNumber {  get; set; }
        [MaxLength(255)]
        public string? CardholderName {  get; set; }
        public DateOnly? ExpDate { get; set; }

        [StringLength(3)]
        public string? Cvc { get; set; }
        #endregion
        #region PayPal Info
        [MaxLength(255)]
        public string? AccountName {  get; set; }
        #endregion

    }
}
