using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Enums
{
    public enum DelayFaultType
    {
        AppointmentBreach,
        LateDispatch,
        Traffic,
        Weather,
        Mechanical,
        Detention,
        Unknown
    }
}
