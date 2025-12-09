namespace LogisticsPlatform.Domain.Enums
{
    public enum OrderDirection
    {
        Inbound = 1, //Items coming to us
        Outbound = 2, //items going outside us
        Transfer = 3 // warehouse to warehouse
    }
}
