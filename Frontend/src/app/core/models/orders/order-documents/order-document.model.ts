export interface OrderDocumentDto {
    id: string;
    documentType: number;
    fileUrl: string;
    isInternal: boolean;
    copyToLoad: boolean;
    createdAt: string;
}
