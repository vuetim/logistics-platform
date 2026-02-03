export interface CreateCustomerContactDto {
    customerId?: string;
    fullName: string;
    email: string;
    phone: string;
    position: string;
    isPrimary: boolean;
    isActive: boolean;
}
