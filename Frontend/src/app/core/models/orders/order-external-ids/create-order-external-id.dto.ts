export interface CreateOrderExternalIdDto {
    type: string;
    value: string;
    relatedParty?: string | null;
    copyToLoad: boolean;
}
