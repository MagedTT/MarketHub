import { Component, computed, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AUTH_CONFIG } from '../../models/auth.config';
import { SessionStoreService } from '../../services/session-store-service';
import { CartStore } from '../../services/stores/cart-store';
import { AuthService } from '../../services/auth-service';
import { NotificationSignalRService } from '../../services/notification-signal-rservice';
import { NotificationService } from '../../services/notification-service';
import { Subject, takeUntil } from 'rxjs';
import { NotificationDto } from '../../models/notification-dto.interface';
import { DatePipe } from '@angular/common';
import { NotificationType } from '../../models/notification-type.enum';

@Component({
  selector: 'app-navbar',
  imports: [DatePipe, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar implements OnInit, OnDestroy {
  private notificationSignalRService = inject(NotificationSignalRService);
  private notificationService = inject(NotificationService);

  notifications: WritableSignal<NotificationDto[]> = signal([]);

  hasUnreadNotifications = computed(() => {
    return this.notifications().some(n => !n.isRead);
  });

  private router = inject(Router);
  private destroy$ = new Subject<void>();
  private config = inject(AUTH_CONFIG);
  private authService = inject(AuthService);
  cartStore = inject(CartStore);
  session = inject(SessionStoreService);

  async ngOnInit(): Promise<void> {
    await this.notificationSignalRService.startConnection();

    this.notificationService.notifications$.pipe(
      takeUntil(this.destroy$)
    ).subscribe((notifications: NotificationDto[]) => {
      this.notifications.set(notifications);
    })

    await this.notificationService.loadNotifications(this.session.user()?.id ?? '');
  }

  async markAllAsRead(event: Event): Promise<void> {
    event.stopPropagation();
    await this.notificationService.markAllNotificationsAsRead(this.session.user()?.id ?? '');
  }

  async navigateToReference(notificationId: string, referenceId: string, notificationType: NotificationType): Promise<void> {

    await this.notificationService.markNotificationAsRead(this.session.user()?.id ?? '', notificationId);

    if (1 <= notificationType && notificationType <= 5) {
      this.router.navigateByUrl('orders');
      return;
    }

    this.router.navigateByUrl('products');
  }

  navigateToLogin() {
    this.router.navigateByUrl(this.config.loginPath);
  }

  navigateToRegister() {
    this.router.navigateByUrl(this.config.registerPath);
  }

  navigateToWishlist() {
    this.router.navigate(['wishlist', this.session.user()?.id]);
  }

  navigateToCart() {
    this.router.navigate(['cart']);
  }

  navigateToProducts() {
    this.router.navigate(['products']);
  }

  logout() {
    this.authService.logout();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}



// getNotificationIconClass(type: NotificationType): string {
//   switch (Number(type)) {
//     case NotificationType.OrderCreated: return 'bi-cart-plus text-primary';
//     case NotificationType.OrderConfirmed: return 'bi-cart-check-fill text-success';
//     case NotificationType.OrderShipped: return 'bi-truck text-info';
//     case NotificationType.OrderDelivered: return 'bi-bag-check-fill text-success';
//     case NotificationType.OrderCancelled: return 'bi-cart-x-fill text-danger';
//     case NotificationType.ProductApproved: return 'bi-box-seam-fill text-success';
//     case NotificationType.ProductRejected: return 'bi-box-arrow-down-left text-danger';
//     case NotificationType.ProductOutOfStock: return 'bi-exclamation-octagon-fill text-warning';
//     case NotificationType.NewReview: return 'bi-star-fill text-warning';
//     case NotificationType.StoreApproved: return 'bi-shop-window text-success';
//     case NotificationType.StoreRejected: return 'bi-x-octagon text-danger';
//     case NotificationType.ReportSubmitted: return 'bi-flag-fill text-warning';
//     case NotificationType.ReportResolved: return 'bi-shield-check text-success';
//     case NotificationType.PromoCodeReceived: return 'bi-ticket-perforated-fill text-purple'; // custom color target
//     case NotificationType.System:
//     default:
//       return 'bi-gear-fill text-secondary';
//   }
// }