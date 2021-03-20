using System.Collections.Generic;

namespace PaymentGateway_Task.Models.API.Response
{
    public class UserDetails
    {
        public UserResponse user { get; set; }
        public List<TransactionResponse> transactions { get; set; }
    }

    public class UserResponse
    {
        public string UserName { get; set; }
        public decimal CreditBalance { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string UserType { get; set; }
        public string BusinessType { get; set; }
        public string PDFName { get; set; }
    }

    public class TransactionResponse
    {
        public string TransactionId { get; set; }
        public string TransactionName { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
    }
}
