import { DatePipe } from '@angular/common';
import { Component, computed, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../../../core/services/notification-service';
import { NotificationSignalRService } from '../../../../core/services/notification-signal-rservice';
import { NotificationDto } from '../../../../core/models/notification-dto.interface';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-seller-navbar',
  imports: [DatePipe, RouterLink],
  templateUrl: './seller-navbar.html',
  styleUrl: './seller-navbar.css',
})
export class SellerNavbar implements OnInit, OnDestroy {
  private subs = new Subscription();
  private notificationSignalRService = inject(NotificationSignalRService);
  private notificationService = inject(NotificationService);

  notifications: WritableSignal<NotificationDto[]> = signal([]);

  hasUnreadNotifications = computed(() => {
    return this.notifications().some(n => !n.isRead);
  });

  async ngOnInit(): Promise<void> {
    await this.notificationSignalRService.startConnection();

    this.subs.add(
      this.notificationService.notifications$.subscribe((notifications: NotificationDto[]) => {
        this.notifications.set(notifications);
      })
    )
  }

  markAllAsRead(event: Event): void {
    event.stopPropagation();
    console.log('Sending transaction execution rules to mark all nodes read upstream.');
  }

  getNotificationRoute(notification: NotificationDto): string[] {
    const type = Number(notification.notificationType);

    if (type >= 1 && type <= 5) {
      return ['/account/orders', notification.reference]; // Navigates directly into historical order view parameters
    }
    if (type === 6 || type === 7 || type === 8) {
      return ['/seller/products/edit', notification.reference];
    }
    if (type === 9) {
      return ['/products', notification.reference];
    }
    if (type === 14) {
      return ['/checkout'];
    }
    return ['/account/dashboard'];
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
