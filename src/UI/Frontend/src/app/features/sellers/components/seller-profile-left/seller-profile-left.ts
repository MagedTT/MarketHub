import { Component, Input, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-seller-profile-left',
  imports: [FormsModule],
  templateUrl: './seller-profile-left.html',
  styleUrl: './seller-profile-left.css',
})
export class SellerProfileLeft {
  @Input() isEditing: boolean = false;

  // Signal serving core record state
  public currentStoreName = signal<string>('MarketHub Tech Shop Ltd.');
  public editableStoreName: string = this.currentStoreName();

  // Getter exposed to extract current text block value securely
  public get storeNameFormValue(): string {
    return this.editableStoreName;
  }

  // Reverts active text fields on cancel actions
  public resetForm(): void {
    this.editableStoreName = this.currentStoreName();
  }

  // Basic empty field string validation checks
  public validate(): boolean {
    return this.editableStoreName.trim().length > 0;
  }
}
