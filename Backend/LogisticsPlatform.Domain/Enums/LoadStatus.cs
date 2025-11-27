namespace LogisticsPlatform.Domain.Enums
{
    public enum LoadStatus : int
    {
        Draft = 0,
        Tendered = 10,
        Covered = 20,
        Dispatched = 30,
        AtPickup = 40,
        InTransit = 50,
        AtDelivery = 60,
        Delivered = 70,
        ReadyForBilling = 80,
        Completed = 90,
        Canceled = 100
    }

}
