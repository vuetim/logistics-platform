export interface CustomerContactDto {
    id: string;
    customerId?: string;
    fullName: string;
    email?: string | null;
    phone?: string | null;

    position: string;
    isPrimary: boolean;
    isActive?: boolean;
}