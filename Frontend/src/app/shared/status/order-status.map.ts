export const ORDER_STATUS_MAP: Record<number, { label: string; class: string }> = {
    1: { label: 'Draft', class: 'badge secondary' },
    2: { label: 'Submitted', class: 'badge primary' },
    3: { label: 'Confirmed', class: 'badge info' },
    4: { label: 'Scheduled', class: 'badge warning' },
    5: { label: 'Dispatched', class: 'badge primary' },
    13: { label: 'Completed', class: 'badge success' },
    99: { label: 'Cancelled', class: 'badge danger' }
};
