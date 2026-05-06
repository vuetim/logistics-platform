import { CustomerBillingDto } from "./customer-billing.dto";

export interface CreateCustomerDto {
    name: string;
    email?: string;
    phone?: string;
    isActive: boolean;
    billing: CustomerBillingDto;
}
