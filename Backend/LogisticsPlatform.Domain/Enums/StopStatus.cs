using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Enums
{
    public enum StopStatus
    {
        Pending = 0,
        EnRoute = 1,
        Arrived = 2,
        Loading = 3,
        Loaded = 4,
        Unloading = 5,
        Unloaded = 6,
        Completed = 7
    }


}
