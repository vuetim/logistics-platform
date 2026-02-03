import { environment } from '../../../environments/environment';

export const API_ENDPOINTS = {
    auth: `${environment.apiBaseUrl}/auth`,
    users: `${environment.apiBaseUrl}/users`,
    customers: `${environment.apiBaseUrl}/customers`,
    customerAddresses: `${environment.apiBaseUrl}/customer-addresses`,
    customerContacts: `${environment.apiBaseUrl}/CustomerContacts`,
    customerNotes: `${environment.apiBaseUrl}/CustomerNotes`,

    // orders: `${environment.apiBaseUrl}/orders`
};
