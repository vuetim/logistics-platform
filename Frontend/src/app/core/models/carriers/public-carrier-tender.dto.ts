export interface PublicCarrierTenderDto {
    assignmentId: string;
    loadNumber: string;
    customerName: string;
    origin: string;
    destination: string;
    carrierName: string;
    offeredRate: number | null;
    currency: string;
    tenderNotes?: string | null;
    tenderExpiresAt?: string | null;
    status: string;
    stops: PublicCarrierTenderStopDto[];
}

export interface PublicCarrierTenderStopDto {
    sequence: number;
    stopType: string;
    locationName: string;
    city: string;
    state: string;
    country: string;
    plannedArrivalFrom?: string | null;
    plannedArrivalTo?: string | null;
}

export interface RespondCarrierTenderDto {
    contactName?: string | null;
    contactEmail?: string | null;
    contactPhone?: string | null;
    notes?: string | null;
}
