using LogisticsPlatform.Application.Interfaces.Services.Loads;

namespace LogisticsPlatform.Application.Services.Loads;

public class LoadNumberGenerator : ILoadNumberGenerator
{
    public string Generate()
        => $"LOAD-{DateTime.UtcNow:MMddyyyy}";
}

