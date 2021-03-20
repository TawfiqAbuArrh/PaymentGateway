using Microsoft.EntityFrameworkCore;
using PaymentGateway_Task.Intrerfaces;
using PaymentGateway_Task.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway_Task.Implementation
{
    public class UserTransactionImplemntation : IUserTransaction
    {
        private readonly PaymentGatewayContext _db;

        public UserTransactionImplemntation(PaymentGatewayContext _db)
        {
            this._db = _db;
        }

        public bool addTransaction(Users user, Models.API.Requests.Transaction request)
        {
            if (user.CreditBalance < decimal.Parse(request.TransactionAmount))
                return false;

            var TransactionId = "Trans-" + Guid.NewGuid().ToString().Substring(6);
            var transaction = new Transaction
            {
                TransactionId = TransactionId,
                TransactionAmount = decimal.Parse(request.TransactionAmount),
                TransactionName = request.TransactionName,
                TransactionTypeId = 1,
                UserId = user.Id
            };
            _db.Transaction.Add(transaction);
            user.CreditBalance -= decimal.Parse(request.TransactionAmount);
            _db.SaveChanges();
            return true;
        }

        public List<Transaction> getUserTransaction(int UserId)
        {
            return _db.Transaction.Where(s => s.UserId == UserId).Include(s => s.TransactionType).ToList();
        }

        public bool isUserExceedTheMaximumAllowance(int UserId, decimal amount)
        {
            var userTransaction = _db.Transaction.Where(s => s.UserId == UserId && s.TransactionTime.Month == DateTime.Now.Month
                && s.TransactionTime.Year == DateTime.Now.Year && (s.TransactionTypeId == 3 || s.TransactionTypeId == 2)).ToList();
            if (userTransaction.Sum(s => s.TransactionAmount) - amount <= -20000)
                return true;
            return false;
        }

        public bool refundTransaction(Users user, Transaction transaction)
        {
            var RefundId = "Refund-" + transaction.TransactionId.Substring(6);

            if (_db.Transaction.Any(s => s.TransactionId.Equals(RefundId)))
                return false;

            if (transaction != null)
            {
                transaction.User.CreditBalance += transaction.TransactionAmount;

                _db.Transaction.Add(new Transaction
                {
                    TransactionAmount = transaction.TransactionAmount * -1,
                    TransactionId = RefundId,
                    TransactionName = transaction.TransactionName,
                    TransactionTypeId = 2,
                    UserId = user.Id
                });
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool withdrawTransaction(Users user, decimal amount)
        {
            if (user.CreditBalance < amount)
                return false;

            var WithdrawId = "Withdraw-" + Guid.NewGuid().ToString().Substring(6);
            user.CreditBalance -= amount;

            _db.Transaction.Add(new Transaction
            {
                TransactionAmount = amount * -1,
                TransactionId = WithdrawId,
                TransactionName = user.UserName + "_Withdraw",
                TransactionTypeId = 3,
                UserId = user.Id
            });
            _db.SaveChanges();
            return true;
        }

        public bool AddBalance(Users user, decimal amount)
        {
            var AddBalanceId = "AddBal-" + Guid.NewGuid().ToString().Substring(6);
            user.CreditBalance += amount;

            _db.Transaction.Add(new Transaction
            {
                TransactionAmount = amount,
                TransactionId = AddBalanceId,
                TransactionName = user.UserName + "_AddBalance",
                TransactionTypeId = 4,
                UserId = user.Id
            });
            _db.SaveChanges();
            return true;
        }
    }
}