import { CustomerBillingDto } from "./customer-billing.dto";

export interface UpdateCustomerDto {
    name?: string;
    email?: string;
    phone?: string;
    IsActive?: boolean;
    billing: CustomerBillingDto;

}