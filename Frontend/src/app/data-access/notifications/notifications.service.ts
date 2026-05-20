import { Injectable } from "@angular/core";
import { NotificationsApi } from "./notifications.api";

@Injectable({ providedIn: 'root' })
export class NotificationsService {
    constructor(private api: NotificationsApi) { }

    getRecent(take = 20) {
        return this.api.getRecent(take);
    }

    getUnreadCount() {
        return this.api.getUnreadCount();
    }

    markRead(id: string) {
        return this.api.markRead(id);
    }

    markAllRead() {
        return this.api.markAllRead();
    }
}
