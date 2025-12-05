namespace LogisticsPlatform.Domain.Enums
{
    public enum ActivityType
    {
        // ===== LOAD =====
        Load_Created = 100,
        Load_StatusChanged = 110,
        Load_StopStatusChanged = 120,
        Load_RddChanged = 130,
        Load_ItemUpdated = 140,

        // ===== ORDER =====
        Order_Created = 200,
        Order_StatusChanged = 210,
        Order_RddChanged = 220,

        // ===== CARRIER / CUSTOMER =====
        Carrier_Updated = 300,
        Customer_Updated = 400
    }
}
