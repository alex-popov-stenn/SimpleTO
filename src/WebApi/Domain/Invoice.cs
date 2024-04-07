namespace WebApi.Domain
{
    public class Invoice
    {
        protected Invoice() { }

        public static Invoice Create(decimal amount, DateTime dueDate)
        {
            var invoice = new Invoice
            {
                Amount = amount,
                DueDate = dueDate,
                Id = Guid.NewGuid()
            };

            return invoice;
        }

        public Guid Id { get; protected internal set; }
        public decimal Amount { get; protected internal set; }
        public DateTime DueDate { get; protected internal set; }
    }
}