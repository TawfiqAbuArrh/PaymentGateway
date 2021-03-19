using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentGateway_Task.Models.DB
{
    public partial class Users
    {
        public Users()
        {
            LoginTokens = new HashSet<LoginTokens>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string Pdf { get; set; }
        public string PdfName { get; set; }
        public int Id { get; set; }
        public bool AdminApproval { get; set; }
        public decimal CreditBalance { get; set; }

        public virtual UsersType UserType { get; set; }
        public virtual ICollection<LoginTokens> LoginTokens { get; set; }
    }
}
