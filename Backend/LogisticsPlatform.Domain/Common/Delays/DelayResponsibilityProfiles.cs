using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Common.Delays
{
 
        public static class DelayResponsibilityProfiles
        {
            public static DelayResponsibilityProfile From(DelayResponsibilityType type)
            {
                return type switch
                {
                    DelayResponsibilityType.Carrier =>
                        new(DelayFaultType.Unknown, DelayResponsibleParty.Carrier),

                    DelayResponsibilityType.Shipper =>
                        new(DelayFaultType.Unknown, DelayResponsibleParty.Shipper),

                    DelayResponsibilityType.ForceMajeure =>
                        new(DelayFaultType.Weather, DelayResponsibleParty.ForceMajeure),
                    // Weather është default i arsyeshëm, jo i detyrueshëm

                    _ =>
                        new(DelayFaultType.Unknown, DelayResponsibleParty.Unknown)
                };
            
        }
    }
}
