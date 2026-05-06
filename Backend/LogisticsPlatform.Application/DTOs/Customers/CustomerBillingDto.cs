using LogisticsPlatform.Domain.Enums;


namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class CustomerBillingDto
    {
        public CustomerPaymentTerms Terms { get; set; }
        public CustomerPaymentMethod Method { get; set; }
        public decimal CreditLimit { get; set; }
        public bool AutoInvoice { get; set; }
    }

}
