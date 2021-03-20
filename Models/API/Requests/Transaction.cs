namespace PaymentGateway_Task.Models.API.Requests
{
    public class Transaction
    {
        public string TransactionName { get; set; }
        public string TransactionAmount { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionId { get; set; }
    }
}