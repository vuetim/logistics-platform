using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Enums
{
    public enum StopStatus
    {
        Pending = 0,     // stop exists but execution not started
        EnRoute = 10,    // driver moving to location
        Arrived = 20,    // arrived but not loaded/unloaded
        Loaded = 30,     // pickup done
        Completed = 40   // delivery done
    }

}
