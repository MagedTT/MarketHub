import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { StoreDto } from '../../models/store-dto.interface';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-seller-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './seller-profile.html',
  styleUrl: './seller-profile.css',
})
export class SellerProfile implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private sellerService = inject(SellerOrdersService);
  private activatedRoute = inject(ActivatedRoute);

  store: WritableSignal<StoreDto | null> = signal(null);
  editable: WritableSignal<boolean> = signal(false);
  logoUrl = signal<string | ArrayBuffer | null>(
    'https://unsplash.com'
  );
  selectedFile: File | null = null;

  // Text Form Field Signals
  storeTitle = signal<string>('MarketHub Tech Shop Ltd.');
  storeDescription =
    'Premium technology hubs rendering custom smartphones, electronic components, and digital accessories live to consumers globally. Established configuration serving regional marketplace networks natively.'
    ;

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const storeId = params.get('id');

      if (storeId) {
        this.getStore(storeId);
      }
    });
  }

  getStore(storeId: string) {
    this.sellerService.getSellerProfile(storeId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.store.set(response);
      console.log(this.store());
    })
  }

  // Handle local image file selections and update the signal node
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files[0]) {
      this.selectedFile = input.files[0];

      const reader = new FileReader();
      reader.onload = () => {
        this.logoUrl.set(reader.result);
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  onSubmit(): void {
    console.log('Saving Data Stack:');
    console.log('Title Payload:', this.storeTitle());
    console.log('Description Payload:', this.storeDescription);
    console.log('Image Data:', this.selectedFile);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
