using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentGateway_Task.Models.DB
{
    public partial class Transaction
    {
        public string TransactionName { get; set; }
        public decimal TransactionAmount { get; set; }
        public int TransactionTypeId { get; set; }
        public int UserId { get; set; }
        public string TransactionId { get; set; }
        public DateTime TransactionTime { get; set; }

        public virtual TransactionTypes TransactionType { get; set; }
        public virtual Users User { get; set; }
    }
}
