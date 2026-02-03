using LogisticsPlatform.Application.Interfaces.Common;

namespace LogisticsPlatform.Infrastructure.Common;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
