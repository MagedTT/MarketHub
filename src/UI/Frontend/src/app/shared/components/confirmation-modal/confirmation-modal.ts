import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, output, signal } from '@angular/core';

@Component({
  selector: 'app-confirmation-modal',
  imports: [CommonModule],
  templateUrl: './confirmation-modal.html',
  styleUrl: './confirmation-modal.css',
})
export class ConfirmationModal {
  // State management signals
  isOpen = signal<boolean>(false);
  title = signal<string>('Confirm Action');
  message = signal<string>('Are you sure you want to proceed?');
  confirmButtonText = signal<string>('Confirm');
  cancelButtonText = signal<string>('Cancel');
  isDangerAction = signal<boolean>(false);

  // Standard @Output decorators with EventEmitters
  @Output() confirmed = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  /**
   * Opens the confirmation dialog layout with a specified configuration
   */
  open(config: {
    title: string;
    message: string;
    confirmText?: string;
    cancelText?: string;
    isDanger?: boolean;
  }): void {
    this.title.set(config.title);
    this.message.set(config.message);
    this.confirmButtonText.set(config.confirmText ?? 'Confirm');
    this.cancelButtonText.set(config.cancelText ?? 'Cancel');
    this.isDangerAction.set(config.isDanger ?? false);

    this.isOpen.set(true);
  }

  onConfirm(): void {
    this.isOpen.set(false);
    this.confirmed.emit(); // Fire confirmation event upstream
  }

  onCancel(): void {
    this.isOpen.set(false);
    this.cancelled.emit(); // Fire cancellation event upstream
  }
}
