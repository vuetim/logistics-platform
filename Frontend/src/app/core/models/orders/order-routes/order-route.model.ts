export interface OrderRouteDto {
    id: string;
    sequence: number;
    stopType: number | string;
    locationName: string;
    addressLine1: string;
    addressLine2?: string | null;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    plannedArrivalFrom?: string | null;
    plannedArrivalTo?: string | null;
    hasTime: boolean;
    copyToLoad: boolean;
    appointmentFrom?: string | null;
    appointmentTo?: string | null;
    stopReference?: string | null;
    appointmentNumber?: string | null;
    notes?: string | null;
    isActive: boolean;
}
