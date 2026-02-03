import { CustomerAddressType } from "./customer-address-type";

export interface UpdateCustomerAddressDto {


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



