export interface NotificationDto {
    id: string;
    title: string;
    message: string;
    type: string;
    actorName: string;
    targetType: string;
    targetId: string;
    route: string;
    isRead: boolean;
    createdAt: string;
    expiresAt: string;
}
