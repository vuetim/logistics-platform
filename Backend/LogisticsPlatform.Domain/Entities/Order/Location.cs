using LogisticsPlatform.Domain.Common;

public class Location : BaseEntity
{
    public string Name { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    //public LocationType Type; // Warehouse, Port, Supplier
}
