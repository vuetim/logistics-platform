using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Enums
{
    public enum BillingStatus
    {
        Draft = 0,
        Sent = 1,
        Delivered = 2,
        PastDue = 3,
        Paid = 4,
        Cancelled = 5
    }
}
