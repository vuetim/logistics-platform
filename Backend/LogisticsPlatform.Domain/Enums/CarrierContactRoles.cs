namespace LogisticsPlatform.Domain.Enums
{
    public static class CarrierContactRoles
    {
        public const string AccountLead = "Account Lead";
        public const string Manager = "Manager";
        public const string Dispatcher = "Dispatcher";
        public const string Driver = "Driver";
        public const string Billing = "Billing";
        public const string Claims = "Claims";
        public const string Other = "Other";

        public static readonly string[] All =
        {
            AccountLead,
            Manager,
            Dispatcher,
            Driver,
            Billing,
            Claims,
            Other
        };
    }
}
