export interface CreateCustomerDto {
    name: string;
    email?: string;
    phone?: string;
    paymentTermsDays: number;
    isActive: boolean;
}
