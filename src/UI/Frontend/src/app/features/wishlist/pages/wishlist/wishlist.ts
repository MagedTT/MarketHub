import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { WishlistService } from '../../services/wishlist-service';
import { Subject, takeUntil } from 'rxjs';
import { wishlistDto } from '../../models/wishlist-dto.interface';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { WishlistCard } from '../../components/wishlist-card/wishlist-card';

@Component({
  selector: 'app-wishlist',
  imports: [WishlistCard],
  templateUrl: './wishlist.html',
  styleUrl: './wishlist.css',
})
export class Wishlist implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private wishlistService = inject(WishlistService);
  private session = inject(SessionStoreService);
  // private activatedRoute = inject(ActivatedRoute);

  // wishlist: WritableSignal<wishlistDto> = signal({
  //   id: '',
  //   userId: '',
  //   Items: []
  // });

  wishlist: WritableSignal<wishlistDto | null> = signal(null);


  ngOnInit(): void {
    this.getWishlist(this.session.user()?.id ?? '');
  }

  getWishlist(userId: string) {
    this.wishlistService.getWishlist(userId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.wishlist.set(response);
    });
  }

  removeWishlistItem(event: { wishlistId: string, productId: string }) {
    const userId = this.session.user()?.id ?? '';
    const obj = {
      userId,
      ...event
    };

    this.wishlistService.removeWishlistItem(obj).subscribe({
      next: () => {
        this.wishlist.update(wishlist => {
          if (!wishlist) return wishlist;

          return {
            ...wishlist,
            wishlistItems: wishlist.wishlistItems.filter(item =>
              item.productId !== event.productId
            )
          }
        });
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
