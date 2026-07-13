import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom, Observable } from 'rxjs';
import { NotificationDto } from '../models/notification-dto.interface';
import { NotificationSignalRService } from './notification-signal-rservice';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private httpClient = inject(HttpClient);
  private notifications = new BehaviorSubject<NotificationDto[]>([]);

  notifications$ = this.notifications.asObservable();

  constructor(private notificationSignalRService: NotificationSignalRService) {
    this.notificationSignalRService.notificationReceived$.subscribe(notification => {
      const currentNotifications = this.notifications.getValue();
      this.notifications.next([notification, ...currentNotifications]);
    });
  }

  async loadNotifications(userId: string): Promise<void> {
    const notifications = await firstValueFrom(this.httpClient.get<NotificationDto[]>(`https://localhost:5001/api/notifications/all/${userId}`));

    this.notifications.next(notifications);
  }

  async markAllNotificationsAsRead(userId: string): Promise<void> {
    await firstValueFrom(this.httpClient.post<any>(`https://localhost:5001/api/notifications/markAllAsRead/${userId}`, {}));

    const notifications = this.notifications.getValue();

    this.notifications.next(notifications.map(notification => ({ ...notification, isRead: true })));
  }

  async markNotificationAsRead(userId: string, notificationId: string): Promise<void> {
    await firstValueFrom(this.httpClient.post<any>(`https://localhost:5001/api/notifications/markNotificationAsRead/${userId}/${notificationId}`, {}));

    const notifications = this.notifications.getValue();

    this.notifications.next(notifications.map(notification =>
      notification.id === notificationId
        ? { ...notification, isRead: true }
        : notification
    ))
  }
}
