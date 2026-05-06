export interface OrderNoteDto {
    id: string;
    orderId: string;
    message: string;
    isInternal: boolean;
    createdByUserId: string;
    createdByName: string;
    createdAt: string;
}
