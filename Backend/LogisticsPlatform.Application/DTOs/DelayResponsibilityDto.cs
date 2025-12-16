using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs
{
  public class DelayResponsibilityDto
{
    public Guid Id { get; set; }
    public Guid LoadId { get; set; }
    public Guid? LoadStopId { get; set; }

    public DelayFaultType FaultType { get; set; }
    public DelayResponsibleParty ResponsibleParty { get; set; }

    public int? MinutesLate { get; set; }
    public string Reason { get; set; } = string.Empty;

    public bool IsManualOverride { get; set; }
    public DateTime CreatedAt { get; set; }
}

}
