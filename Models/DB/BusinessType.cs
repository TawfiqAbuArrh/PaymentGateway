using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentGateway_Task.Models.DB
{
    public partial class BusinessType
    {
        public BusinessType()
        {
            BusinessProfile = new HashSet<BusinessProfile>();
        }

        public int BusinessId { get; set; }
        public string BusinessName { get; set; }

        public virtual ICollection<BusinessProfile> BusinessProfile { get; set; }
    }
}
