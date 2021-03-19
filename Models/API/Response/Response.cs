namespace PaymentGateway_Task.Models.API.Response
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object ResponseResults { get; set; }
    }
}