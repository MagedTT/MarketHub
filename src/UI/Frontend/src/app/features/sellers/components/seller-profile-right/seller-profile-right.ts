import { Component, Input, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-seller-profile-right',
  imports: [FormsModule],
  templateUrl: './seller-profile-right.html',
  styleUrl: './seller-profile-right.css',
})
export class SellerProfileRight {
  @Input() isEditing: boolean = false;

  // Signals serving core data records profiles configuration metrics
  public currentOwnerName = signal<string>('Maged Tarek');
  public currentOwnerEmail = signal<string>('maged.tarek@markethub.com');

  public editableOwnerName: string = this.currentOwnerName();
  public editableOwnerEmail: string = this.currentOwnerEmail();

  // Getters exposed for the parent component wrapper data harvesting loops
  public get ownerNameFormValue(): string { return this.editableOwnerName; }
  public get ownerEmailFormValue(): string { return this.editableOwnerEmail; }

  // Reverts inputs state back to signals value on cancellation hooks triggers
  public resetForm(): void {
    this.editableOwnerName = this.currentOwnerName();
    this.editableOwnerEmail = this.currentOwnerEmail();
  }

  // Structural checks validating email parameters patterns
  public validate(): boolean {
    return this.editableOwnerName.trim().length > 0 && this.editableOwnerEmail.includes('@');
  }
}
