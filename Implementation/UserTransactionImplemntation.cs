using Microsoft.EntityFrameworkCore;
using PaymentGateway_Task.Intrerfaces;
using PaymentGateway_Task.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway_Task.Implementation
{
    public class UserTransactionImplemntation : IUserTransaction
    {
        private readonly PaymentGatewayContext _db;
        public UserTransactionImplemntation(PaymentGatewayContext _db)
        {
            this._db = _db;
        }

        public string addTransaction(int UserId, Transaction request)
        {
            var TransactionId = "Trans-" + Guid.NewGuid().ToString().Substring(6);
            var transaction = new Transaction
            {
                TransactionId = TransactionId,
                TransactionAmount = request.TransactionAmount,
                TransactionName = request.TransactionName,
                TransactionTypeId = request.TransactionTypeId,
                UserId = UserId
            };
            _db.Transaction.Add(transaction);
            _db.SaveChanges();
            
            return TransactionId;
        }

        public List<Transaction> getUserTransaction(int UserId)
        {
            return _db.Transaction.Where(s => s.UserId == UserId).Include(s => s.TransactionType).ToList();
        }

        public bool refundTransaction(int UserId, string transactionID)
        {
            throw new NotImplementedException();
        }

        public bool withdrawTransaction(int UserId, string transactionID)
        {
            throw new NotImplementedException();
        }
    }
}
