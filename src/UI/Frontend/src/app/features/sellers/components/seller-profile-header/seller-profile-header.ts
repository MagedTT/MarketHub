import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, signal, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-seller-profile-header',
  imports: [FormsModule, CommonModule],
  templateUrl: './seller-profile-header.html',
  styleUrl: './seller-profile-header.css',
})
export class SellerProfileHeader implements OnChanges {
  @Input() isEditing: boolean = false;
  @Output() toggleEdit = new EventEmitter<boolean>();
  @Output() saveProfile = new EventEmitter<void>();

  public currentDescription = signal<string>('Premium technology hubs rendering custom smartphones live to consumers globally.');
  public editableDescription: string = ''; // Leave blank initially
  public logoUrlValue: string = 'https://unsplash.com';

  // Detects when [isEditing] transitions from false to true or true to false
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isEditing']) {
      this.resetForm();
    }
  }

  public get storeDescFormValue(): string { return this.editableDescription; }

  public resetForm(): void {
    this.editableDescription = this.currentDescription();
  }

  public validate(): boolean {
    return this.editableDescription.trim().length > 0;
  }

  public editToggled(status: boolean): void {
    console.log('Edit profile click registered. New state flag:', status);
    this.toggleEdit.emit(status);
  }

  public onLogoChanged(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files[0]) {
      const reader = new FileReader();
      reader.onload = () => this.logoUrlValue = reader.result as string;
      reader.readAsDataURL(fileInput.files[0]);
    }
  }
}