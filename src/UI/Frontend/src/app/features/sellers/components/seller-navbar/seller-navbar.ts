import { DatePipe } from '@angular/common';
import { Component, computed, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { NotificationService } from '../../../../core/services/notification-service';
import { NotificationSignalRService } from '../../../../core/services/notification-signal-rservice';
import { NotificationDto } from '../../../../core/models/notification-dto.interface';
import { Router, RouterLink } from '@angular/router';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { NotificationType } from '../../../../core/models/notification-type.enum';

@Component({
  selector: 'app-seller-navbar',
  imports: [DatePipe, RouterLink],
  templateUrl: './seller-navbar.html',
  styleUrl: './seller-navbar.css',
})
export class SellerNavbar implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private notificationSignalRService = inject(NotificationSignalRService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  session = inject(SessionStoreService);

  notifications: WritableSignal<NotificationDto[]> = signal([]);

  hasUnreadNotifications = computed(() => {
    return this.notifications().some(n => !n.isRead);
  });

  async ngOnInit(): Promise<void> {
    await this.notificationSignalRService.startConnection();

    this.notificationService.notifications$.pipe(
      takeUntil(this.destroy$)
    ).subscribe((notifications: NotificationDto[]) => {
      this.notifications.set(notifications);
    });

    await this.notificationService.loadNotifications(this.session.user()?.id ?? '');
  }

  async markAllAsRead(event: Event): Promise<void> {
    event.stopPropagation();
    await this.notificationService.markAllNotificationsAsRead(this.session.user()?.id ?? '');
  }

  async navigateToReference(notificationId: string, referenceId: string, notificationType: NotificationType): Promise<void> {
    const type = Number(notificationType);

    await this.notificationService.markNotificationAsRead(this.session.user()?.id ?? '', notificationId);

    if (1 <= type && type <= 5) {
      this.router.navigate(['/seller-order-details', referenceId]);
      return;
    }
    if (6 <= type && type <= 8) {
      this.router.navigate(['/seller-product-details', referenceId]);
      return;
    }
    if (type === 9) {
      this.router.navigate(['/seller-review', referenceId]);
      return;
    }

    this.router.navigate(['/seller-dashboard']);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
