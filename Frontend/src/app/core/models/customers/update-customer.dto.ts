
export interface UpdateCustomerDto {
    name?: string;
    email?: string;
    phone?: string;
    paymentTermDays: number;
    IsActive?: boolean;

}