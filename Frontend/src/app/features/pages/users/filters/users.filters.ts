import { FilterConfig } from "../../../../shared/filters/filter-builder/filter.types";

export const USER_FILTERS: FilterConfig[] = [
    {
        key: 'search',
        label: 'Search users',
        type: 'text'
    },
    {
        key: 'role',
        label: 'Role',
        type: 'dropdown',
        options: [
            { label: 'Admin', value: 'Admin' },
            { label: 'Operator', value: 'Operator' },
            { label: 'Dispatcher', value: 'Dispatcher' },
            { label: 'Broker', value: 'Broker' }
        ]
    },
    {
        key: 'isActive',
        label: 'Status',
        type: 'dropdown',
        options: [
            { label: 'Active', value: true },
            { label: 'Inactive', value: false }
        ]
    }
];
