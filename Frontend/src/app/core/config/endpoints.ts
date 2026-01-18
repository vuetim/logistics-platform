import { environment } from '../../../environments/environment';

export const API_ENDPOINTS = {
    auth: `${environment.apiBaseUrl}/auth`,
    users: `${environment.apiBaseUrl}/users`
    // loads: `${environment.apiBaseUrl}/loads`,
    // orders: `${environment.apiBaseUrl}/orders`
};
