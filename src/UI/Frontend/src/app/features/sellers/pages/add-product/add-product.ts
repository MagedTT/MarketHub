import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from "@angular/router";
import { SellerOrdersService } from '../../services/seller-orders-service';
import { BrandDto } from '../../models/brand-dto.interface';
import { CategoryDto } from '../../models/category-dto.interface';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-product',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './add-product.html',
  styleUrl: './add-product.css',
})
export class AddProduct implements OnInit, OnDestroy {
  private sellerService = inject(SellerOrdersService);
  private session = inject(SessionStoreService);
  private destory$ = new Subject<void>();

  brands: WritableSignal<BrandDto[]> = signal([]);
  categories: WritableSignal<CategoryDto[]> = signal([]);

  public productForm!: FormGroup;

  public selectedFiles: File[] = [];

  public imagePreviews: WritableSignal<string[]> = signal(['', '', '', '', '']);

  // Mapping labels metadata for loop operations
  public photoSlots = [
    { label: 'Main Photo', index: 0 },
    { label: 'Order #2', index: 1 },
    { label: 'Order #3', index: 2 },
    { label: 'Order #4', index: 3 },
    { label: 'Order #5', index: 4 }
  ];

  constructor(private fb: FormBuilder) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      price: [null, [Validators.required, Validators.min(0.01)]],
      type: ['', [Validators.required]],
      specifications: ['', [Validators.required]],
      brandId: ['', [Validators.required]],
      isActive: [true],
      availableQuantityInStock: [null, [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.sellerService.getBrandsWithIdsAndNames().subscribe(response => {
      this.brands.set(response);
    });

    this.sellerService.getCategories().subscribe(response => {
      this.categories.set(response);
    });
  }

  /**
   * Captures chosen image file and extracts localized canvas thumbnail tracking preview path
   */
  public onImageSelected(event: Event, slotIndex: number): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      const file = inputElement.files[0];
      this.selectedFiles[slotIndex] = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreviews.update(previews => {
          const updatedPreviews = [...previews];
          updatedPreviews[slotIndex] = reader.result as string;
          return updatedPreviews;
        });
      };
      reader.readAsDataURL(file);
    }
  }

  convertSpecificationsToJson(specifications: string): string {
    const result: Record<string, string> = {};

    specifications
      .split('|')
      .map(item => item.trim())
      .forEach(item => {
        const [key, value] = item.split(':').map(part => part.trim());

        if (key && value) {
          result[key] = value;
        }
      });

    return JSON.stringify(result);
  }

  public onSubmit(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    const formData = new FormData();
    const formValues = this.productForm.value;

    formData.append('name', formValues.name);
    formData.append('description', formValues.description);
    formData.append('specifications', this.convertSpecificationsToJson(formValues.specifications));
    formData.append('brandId', formValues.brandId);
    formData.append('storeId', this.session.user()?.storeId ?? '');
    formData.append('type', formValues.type);
    formData.append('price', formValues.price.toString());
    formData.append('isActive', formValues.isActive.toString());
    formData.append('availableQuantityInStock', formValues.availableQuantityInStock.toString());

    for (const file of this.selectedFiles) {
      formData.append('images', file, file.name);
    }

    formData.forEach(value => console.log(value));

    this.sellerService.createProduct(formData).pipe(
      takeUntil(this.destory$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      console.log(response);
    });
  }

  ngOnDestroy(): void {
    this.destory$.next();
    this.destory$.complete();
  }
}