export interface UpdateOrderExternalIdDto {
    type?: string | null;
    value?: string | null;
    relatedParty?: string | null;
    copyToLoad?: boolean | null;
}
