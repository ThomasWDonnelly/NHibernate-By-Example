using System.Collections.Generic;

namespace WebApplication1.Code.Model
{
    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual IList<CreditCard> CreditCards { get; set; }

        public Customer()
        {
            CreditCards = new List<CreditCard>();
        }
    }
}