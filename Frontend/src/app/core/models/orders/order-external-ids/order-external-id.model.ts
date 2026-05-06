export interface OrderExternalIdDto {
    id: string;
    orderId: string;
    type: string;
    value: string;
    relatedParty?: string | null;
    copyToLoad: boolean;
    createdAt: string;
}
