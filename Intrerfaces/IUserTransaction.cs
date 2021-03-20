using PaymentGateway_Task.Models.DB;
using System.Collections.Generic;

namespace PaymentGateway_Task.Intrerfaces
{
    public interface IUserTransaction
    {
        public List<Transaction> getUserTransaction(int UserId);
        public bool addTransaction(Users user, Models.API.Requests.Transaction request);
        public bool withdrawTransaction(Users user, decimal amount);
        public bool refundTransaction(Users user, Transaction transaction);
        public bool isUserExceedTheMaximumAllowance(int UserId, decimal amount);
        public bool AddBalance(Users user, decimal amount);
    }
}