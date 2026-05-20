import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { NotificationDto } from "../../core/models/notifications/notification.dto";

@Injectable({ providedIn: 'root' })
export class NotificationsApi {
    constructor(private http: HttpClient) { }

    getRecent(take = 20) {
        return this.http.get<NotificationDto[]>(API_ENDPOINTS.notifications, {
            params: { take }
        });
    }

    getUnreadCount() {
        return this.http.get<{ count: number }>(`${API_ENDPOINTS.notifications}/unread-count`);
    }

    markRead(id: string) {
        return this.http.patch<void>(`${API_ENDPOINTS.notifications}/${id}/read`, {});
    }

    markAllRead() {
        return this.http.patch<void>(`${API_ENDPOINTS.notifications}/read-all`, {});
    }
}
