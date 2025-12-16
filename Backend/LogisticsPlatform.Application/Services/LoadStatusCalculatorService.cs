using LogisticsPlatform.Application.Interfaces.Services.Loads;
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
                return load.Status;

            var pickupStops = stops
                .Where(s => s.StopType == StopType.Pickup)
                .ToList();

            var deliveryStops = stops
                .Where(s => s.StopType == StopType.Delivery)
                .ToList();

            // ================================
            // 1️⃣ ALL deliveries unloaded → Delivered
            // ================================
            if (deliveryStops.Any() &&
                deliveryStops.All(s => s.Status == StopStatus.Completed))
                return LoadStatus.Delivered;

            // ================================
            // 2️⃣ Arrived at delivery → AtDelivery
            // ================================
            if (deliveryStops.Any(s => s.Status == StopStatus.Arrived))
                return LoadStatus.AtDelivery;

            // ================================
            // 3️⃣ Arrived at ANY pickup → AtPickup
            // (manual dispatcher confirmation)
            // ================================
            if (pickupStops.Any(s => s.Status == StopStatus.Arrived))
                return LoadStatus.AtPickup;

            // ================================
            // 4️⃣ Loaded at pickup
            // ================================
            if (pickupStops.Any(s => s.Status == StopStatus.Loaded))
            {
                // A ka pickup tjetër që s’është completed?
                var remainingPickups =
                    pickupStops.Any(s => s.Status != StopStatus.Completed &&
                                          s.Status != StopStatus.Loaded);

                // nëse ka pickup tjetër → on the way to pickup tjetër
                if (remainingPickups)
                    return LoadStatus.Dispatched;

                // përndryshe → po shkon te delivery
                return LoadStatus.InTransit;
            }

            // ================================
            // 5️⃣ On the way (mid-stops)
            // ================================
            if (stops.Any(s => s.Status == StopStatus.EnRoute))
                return LoadStatus.Dispatched;

            // ================================
            // 6️⃣ Default
            // ================================
            return LoadStatus.Dispatched;
        }
    }
}
