using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Extensions
{
    public static class LoadFinancialExtensions
    {
        public static void RecalculateFinancials(this Load load)
        {
            load.CustomerRate ??= 0;
            load.CarrierRate ??= 0;

        }
    }
}
