namespace LogisticsPlatform.Domain.Enums
{
    public enum LoadDocumentType : int
    {
        POD = 0,          // Proof of Delivery
        BOL = 1,          // Bill of Lading
        RateConfirmation = 2,
        Invoice = 3,
        Other = 99
    }

}
