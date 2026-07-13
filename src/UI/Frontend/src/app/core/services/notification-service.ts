import { Injectable } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { NotificationDto } from '../models/notification-dto.interface';
import { NotificationSignalRService } from './notification-signal-rservice';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private subs = new Subscription();
  private notifications = new BehaviorSubject<NotificationDto[]>([]);

  notifications$ = this.notifications.asObservable();

  constructor(private notificationSignalRService: NotificationSignalRService) {
    this.subs.add(
      this.notificationSignalRService.notificationReceived$.subscribe(notification => {
        const currentNotifications = this.notifications.getValue();
        this.notifications.next([notification, ...currentNotifications]);
      })
    );
  }
}
