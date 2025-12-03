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
        Arrived = 1,
        Loaded = 2,
        Unloaded = 3,
        Completed = 4
    }
}
