namespace WebApplication1.Code.Model
{
    public class CreditCard
    {
        public virtual int Id { get; set; }
        public virtual string CreditCardNumber { get; set; }
        public virtual string EncryptedCreditCardNumber { get; set; }
    }
}