using PaymentGateway_Task.Models.DB;
using System.Collections.Generic;

namespace PaymentGateway_Task.Intrerfaces
{
    public interface IUserTransaction
    {
        public List<Transaction> getUserTransaction(int UserId);
        public string addTransaction(int UserId, Transaction request);
        public bool withdrawTransaction(int UserId, string transactionID);
        public bool refundTransaction(int UserId, string transactionID);
    }
}