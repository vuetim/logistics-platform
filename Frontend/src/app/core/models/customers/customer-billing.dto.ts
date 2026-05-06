import { CustomerPaymentMethod } from "../../enums/customers/customer-payment-method.enum";
import { CustomerPaymentTerms } from "../../enums/customers/customer-payment-terms.enum";


export interface CustomerBillingDto {
    terms: CustomerPaymentTerms;
    method: CustomerPaymentMethod;
    creditLimit: number;
    autoInvoice: boolean;
}
