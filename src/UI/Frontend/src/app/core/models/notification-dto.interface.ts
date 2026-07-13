import { NotificationType } from "./notification-type.enum";

export interface NotificationDto {
    id: string;
    userId: string;
    reference: string;
    title: string;
    message: string;
    notificationType: NotificationType;
    isRead: boolean;
    createdAt: Date;
};