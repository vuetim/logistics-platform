export interface QueryParameters {
    page: number;
    pageSize: number;
    search?: string;
    sortBy?: string;
    sortDir?: 'asc' | 'desc';
}
