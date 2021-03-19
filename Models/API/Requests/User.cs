namespace PaymentGateway_Task.Models.API.Requests
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CreditBalance { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string UserType { get; set; }
        public string BusinessType { get; set; }
    }
}