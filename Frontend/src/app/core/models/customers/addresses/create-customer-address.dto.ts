import { CustomerAddressType } from "./customer-address-type";

export interface CreateCustomerAddressDto {
    customerId?: string;
    addressLine1: string;
    addressLine2?: string;
    city: string;
    state?: string;
    country: string;
    postalCode?: string;
    type: CustomerAddressType;
    isPrimary: boolean;
    isActive: boolean;
}
