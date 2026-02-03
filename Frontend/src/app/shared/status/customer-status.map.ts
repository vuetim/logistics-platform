export const CUSTOMER_STATUS_MAP: Record<
    'true' | 'false',
    { label: string; class: string }
> = {
    true: {
        label: 'Active',
        class: 'badge success'
    },
    false: {
        label: 'Inactive',
        class: 'badge danger'
    }
};
