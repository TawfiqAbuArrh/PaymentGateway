using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentGateway_Task.Models.DB
{
    public partial class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Passwords { get; set; }
    }
}
