import { Component, computed, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AUTH_CONFIG } from '../../models/auth.config';
import { SessionStoreService } from '../../services/session-store-service';
import { CartStore } from '../../services/stores/cart-store';
import { AuthService } from '../../services/auth-service';
import { NotificationSignalRService } from '../../services/notification-signal-rservice';
import { NotificationService } from '../../services/notification-service';
import { Subscription } from 'rxjs';
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
  // constructor(private router: Router, @Inject(AUTH_CONFIG) private config: AuthConfig) { }
  private subs = new Subscription();
  private notificationSignalRService = inject(NotificationSignalRService);
  private notificationService = inject(NotificationService);

  notifications: WritableSignal<NotificationDto[]> = signal([]);

  hasUnreadNotifications = computed(() => {
    return this.notifications().some(n => !n.isRead);
  });

  private router = inject(Router);
  private config = inject(AUTH_CONFIG);
  private authService = inject(AuthService);
  cartStore = inject(CartStore);
  session = inject(SessionStoreService);

  async ngOnInit(): Promise<void> {
    await this.notificationSignalRService.startConnection();

    this.subs.add(
      this.notificationService.notifications$.subscribe((notifications: NotificationDto[]) => {
        this.notifications.set(notifications);
      })
    );
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

  markAllAsRead(event: Event): void {
    event.stopPropagation(); // Avoid closing out dropdown panels unexpectedly 
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
    this.subs.unsubscribe();
  }
}
