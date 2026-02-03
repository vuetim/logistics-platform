export interface CustomerNoteDto {
    id: string;
    customerId: string;

    title: string;
    message: string;

    createdByUserId: string;
    createdByName: string;

    createdAt: string;
}
