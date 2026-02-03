export const CUSTOMER_ADDRESS_TYPES = {
    Main: 'Main',
    Billing: 'Billing',
    Shipping: 'Shipping',
    Work: 'Work',
    Warehouse: 'Office',
    Other: 'Other'

} as const;

export type CustomerAddressType =
    typeof CUSTOMER_ADDRESS_TYPES[keyof typeof CUSTOMER_ADDRESS_TYPES];
