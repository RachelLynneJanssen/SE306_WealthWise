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
        public int Id { get; set; }
        public string Name { get; set; }
        public PaymentType Type { get; set; }
        #region CC Info
        [StringLength(16)]
        public string? CardNumber {  get; set; }
        public string? CardholderName {  get; set; }
        public DateOnly? ExpDate { get; set; }

        [StringLength(3)]
        public string? Cvc { get; set; }
        #endregion
        #region PayPal Info
        public string? AccountName {  get; set; }
        #endregion
        public int UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

    }
}
