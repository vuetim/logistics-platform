namespace LogisticsPlatform.Domain.Enums
{



    public enum LoadDocumentType
    {
        BOL = 1,                // Bill of Lading
        POD = 2,                // Proof of Delivery
        RateConfirmation = 3,   // Carrier rate confirmation
        InvoiceCustomer = 4,    // Customer invoice PDF
        SettlementCarrier = 5,  // Carrier settlement PDF
        LumperReceipt = 6,      // Lumper / unloading fees
        AccessorialReceipt = 7, // Extra charges proof
        WeightTicket = 8,       // Scale ticket
        Other = 99
    }
}