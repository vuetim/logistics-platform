export interface TableColumn<T> {
    key: keyof T | string;
    label: string;
    sortable?: boolean;
    formatter?: (row: T) => string;
    classFn?: (row: T) => string;
    width?: string;
}
export type ButtonVariant = 'primary' | 'secondary' | 'danger' | 'ghost';

export interface TableAction<T> {
    label: string;
    icon?: string;
    visible?: (row: T) => boolean;
    variant?: ButtonVariant | null;
    handler: (row: T) => void;
}
