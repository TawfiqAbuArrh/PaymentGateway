namespace PaymentGateway_Task.Models.API.Requests
{
    public class Transaction
    {
        public string TransactionName { get; set; }
        public decimal TransactionAmount { get; set; }
        public int TransactionTypeId { get; set; }
    }
}