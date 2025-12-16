using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Common.Delays
{
    public record DelayResponsibilityProfile(
       DelayFaultType FaultType,
       DelayResponsibleParty ResponsibleParty
   );
}
