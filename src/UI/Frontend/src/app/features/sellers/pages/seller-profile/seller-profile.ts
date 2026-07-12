import { Component, signal, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-seller-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './seller-profile.html',
  styleUrl: './seller-profile.css',
})
export class SellerProfile {
  editable: WritableSignal<boolean> = signal(true);
  logoUrl = signal<string | ArrayBuffer | null>(
    'https://unsplash.com'
  );
  selectedFile: File | null = null;

  // Text Form Field Signals
  storeTitle = signal<string>('MarketHub Tech Shop Ltd.');
  storeDescription = signal<string>(
    'Premium technology hubs rendering custom smartphones, electronic components, and digital accessories live to consumers globally. Established configuration serving regional marketplace networks natively.'
  );

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
    console.log('Description Payload:', this.storeDescription());
    console.log('Image Data:', this.selectedFile);
  }
}
