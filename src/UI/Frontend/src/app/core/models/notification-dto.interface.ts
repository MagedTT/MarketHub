import { NotificationType } from "./notification-type.enum";

export interface NotificationDto {
    id: string;
    userId: string;
    referenceId: string;
    title: string;
    message: string;
    type: NotificationType;
    isRead: boolean;
    createdAt: Date;
};