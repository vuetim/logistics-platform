import { SidebarItem } from './sidebar.model';

export const SIDEBAR_ITEMS: SidebarItem[] = [
    {
        label: 'Dashboard',
        route: '/dashboard',
        icon: 'dashboard'
    },
    {
        label: 'Orders',
        icon: 'shopping_cart',
        children: [
            { label: 'Create Order', route: '/orders/create', icon: 'add', disabled: false },
            { label: 'Orders List', route: '/orders', icon: 'list', disabled: false }
        ]
    },
    {
        label: 'Loads',
        icon: 'local_shipping',
        children: [
            { label: 'Loads List', route: '/loads', icon: 'list', disabled: false },
            { label: 'Carrier Offers', route: '/loads/carrier-offers', icon: 'local_offer', disabled: false, permission: 'CarrierOffer_View_All' }
        ]
    },
    { label: 'Financials', route: '/financials', icon: 'request_quote', disabled: false, permission: 'Financial_View' },
    { label: 'Customers', route: 'admin/customers', icon: 'groups', disabled: false },
    { label: 'Carriers', route: '/carriers', icon: 'warehouse', disabled: true },
    { label: 'Documents', route: '/documents', icon: 'description', disabled: false }
];

