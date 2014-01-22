namespace WebApplication1.Code.Model
{
    public class Order
    {
        public virtual int Id { get; set; }
        public virtual string NameOfThingBought { get; set; }
        public virtual Money Price { get; set; }
    }
}