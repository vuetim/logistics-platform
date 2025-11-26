namespace LogisticsPlatform.Domain.Enums
{
    public static class CustomerContactRoles
    {
        public const string AccountLead = "Account Lead";
        public const string Manager = "Manager";
        public const string Billing = "Billing";
        public const string Claims = "Claims";
        public const string Other = "Other";

        public static readonly string[] All =
        {
            AccountLead,
            Manager,
            Billing,
            Claims,
            Other
        };
    }
}
