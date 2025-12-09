using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services
{
    public class LoadStatusCalculatorService : ILoadStatusCalculatorService
    {
        public LoadStatus Calculate(Load load)
        {
            var stops = load.Stops
                .OrderBy(s => s.Sequence)
                .ToList();

            if (!stops.Any())
                return LoadStatus.Draft;

            var pickupStops = stops.Where(s => s.StopType == StopType.Pickup).ToList();
            var deliveryStops = stops.Where(s => s.StopType == StopType.Delivery).ToList();

            // ✅ 1. All delivery completed → Delivered
            if (deliveryStops.Any() &&
                deliveryStops.All(s => s.Status == StopStatus.Completed))
            {
                return LoadStatus.Delivered;
            }

            // ✅ 2. At delivery (arrived but not completed)
            if (deliveryStops.Any(s =>
                s.Status == StopStatus.EnRoute ||
                s.Status == StopStatus.Arrived))
            {
                return LoadStatus.AtDelivery;
            }

            // ✅ 3. In transit (picked up but not yet at delivery)
            if (pickupStops.Any(s => s.Status == StopStatus.Loaded))
            {
                return LoadStatus.InTransit;
            }

            // ✅ 4. At pickup (on the way or arrived)
            if (pickupStops.Any(s =>
                s.Status == StopStatus.EnRoute ||
                s.Status == StopStatus.Arrived))
            {
                return LoadStatus.AtPickup;
            }

            // ✅ 5. Fallback
            return LoadStatus.Dispatched;
        }
    }
}
