using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentGateway_Task.Models.DB
{
    public partial class BusinessProfile
    {
        public int UserId { get; set; }
        public int BusinessTypeId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Pdf { get; set; }
        public string PdfName { get; set; }

        public virtual BusinessType BusinessType { get; set; }
        public virtual Users User { get; set; }
    }
}
