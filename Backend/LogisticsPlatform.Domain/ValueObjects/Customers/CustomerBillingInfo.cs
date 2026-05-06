using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

public class CustomerBillingInfo : ValueObject
{
    public CustomerPaymentTerms Terms { get; private set; }
    public CustomerPaymentMethod Method { get; private set; }
    public decimal CreditLimit { get; private set; }
    public bool AutoInvoice { get; private set; }

    private CustomerBillingInfo() { }

    public CustomerBillingInfo(
        CustomerPaymentTerms terms,
        CustomerPaymentMethod method,
        decimal creditLimit,
        bool autoInvoice)
    {
        Terms = terms;
        Method = method;
        CreditLimit = creditLimit;
        AutoInvoice = autoInvoice;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Terms;
        yield return Method;
        yield return CreditLimit;
        yield return AutoInvoice;
    }
}
