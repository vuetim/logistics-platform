export interface FilterOption {
    label: string;
    value: any;
}

export type FilterType = 'text' | 'dropdown';

export interface FilterConfig {
    key: string;
    label: string;
    type: FilterType;
    options?: FilterOption[];
}
