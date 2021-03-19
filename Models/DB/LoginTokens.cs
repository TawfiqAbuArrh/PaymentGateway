using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentGateway_Task.Models.DB
{
    public partial class LoginTokens
    {
        public string Token { get; set; }
        public int? UserId { get; set; }
        public DateTime TokenTime { get; set; }
        public DateTime? TokenExpiration { get; set; }

        public virtual Users User { get; set; }
    }
}
