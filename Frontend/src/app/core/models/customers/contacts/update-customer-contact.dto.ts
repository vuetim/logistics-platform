

export interface UpdateCustomerContactDto {
    fullName?: string;
    email: string;
    phone: string;
    position?: string;
    isPrimary: boolean;
    isActive: boolean;
}